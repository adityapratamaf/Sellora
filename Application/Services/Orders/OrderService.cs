using AutoMapper;
using Domain.Entities.Orders;
using Domain.Interfaces.Carts;
using Domain.Interfaces.Orders;
using Domain.Interfaces.Payments;
using Domain.Interfaces.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTO.Orders;

namespace Application.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderResponse> CheckoutAsync(CheckoutRequest request);
        Task<bool> ConfirmPaymentAsync(Guid orderId);
        Task<bool> CancelAsync(Guid orderId);
        Task<OrderResponse?> GetByIdAsync(Guid orderId);
    }

    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _log;

        private static readonly TimeSpan PaymentTtl = TimeSpan.FromMinutes(15);

        public OrderService(
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IOrderRepository orderRepository,
            IReservationRepository reservationRepository,
            IProductRepository productRepository,
            IPaymentRepository paymentRepository,
            IMapper mapper,
            ILogger<OrderService> log)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _reservationRepository = reservationRepository;
            _productRepository = productRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _log = log;
        }

        public async Task<OrderResponse> CheckoutAsync(CheckoutRequest request)
        {
            var now = DateTime.UtcNow;

            await _reservationRepository.CleanupExpiredAsync(now);

            var payment = await EnsureActivePaymentAsync(request.PaymentId);

            var cart = await EnsureCartForCheckoutAsync(request.UserId, request.CartId);

            // ambil item terurut agar mengurangi resiko deadlock saat lock row product
            var items = cart.Items.OrderBy(x => x.ProductId).ToList();

            // validasi stok dengan menghitung reserved aktif
            foreach (var ci in items)
                await EnsureStockAvailableAsync(ci.ProductId, ci.Quantity, now);

            // buat order + snapshot items
            var order = CreateOrderDraft(request, now, items);

            await _orderRepository.CreateAsync(order);

            // reserve stok
            var reservations = order.Items.Select(oi => new StockReservation
            {
                OrderId = order.Id,
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                ReservedUntil = order.ExpiresAt
            });

            await _reservationRepository.CreateManyAsync(reservations);

            // hapus bersihkan cart
            await _cartItemRepository.DeleteByCartIdAsync(cart.Id);

            _log.LogInformation("Order Created (PendingPayment): {OrderId}", order.Id);

            // map response (payment name diambil dari entity payment yang sudah kita validasi)
            var response = _mapper.Map<OrderResponse>(order);
            response.PaymentName = payment.Name;
            return response;
        }

        public async Task<bool> ConfirmPaymentAsync(Guid orderId)
        {
            var now = DateTime.UtcNow;

            // idealnya repo menyediakan method "GetByIdWithItemsAsync(orderId, tracked:true)"
            // biar service gak perlu EF Context langsung.
            var order = await _orderRepository.GetByIdWithItemsAsync(orderId); // buat di repo kamu
            if (order is null) return false;

            EnsurePayable(order);

            if (order.ExpiresAt <= now)
            {
                order.Status = "Expired";
                order.UpdatedAt = now;

                await _orderRepository.UpdateAsync(order);
                await _reservationRepository.ReleaseByOrderIdAsync(orderId);

                return false;
            }

            // lock product rows dan deduct stock saat paid
            foreach (var item in order.Items.OrderBy(x => x.ProductId))
            {
                var p = await _productRepository.GetByIdForUpdateAsync(item.ProductId);
                if (p is null) throw new Exception("Product Not Found");

                if (p.Stock < item.Quantity)
                    throw new InvalidOperationException("Stock not enough at payment time");

                p.Stock -= item.Quantity;
                await _productRepository.UpdateAsync(p);
            }

            await _reservationRepository.ReleaseByOrderIdAsync(orderId);

            order.Status = "Paid";
            order.UpdatedAt = now;
            await _orderRepository.UpdateAsync(order);

            _log.LogInformation("Order Paid and Stock Deducted: {OrderId}", orderId);

            return true;
        }

        public async Task<bool> CancelAsync(Guid orderId)
        {
            var now = DateTime.UtcNow;

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null) return false;

            if (order.Status == "Paid")
                throw new Exception("Paid order cannot be cancelled");

            order.Status = "Cancelled";
            order.UpdatedAt = now;

            await _orderRepository.UpdateAsync(order);
            await _reservationRepository.ReleaseByOrderIdAsync(orderId);

            _log.LogInformation("Order Cancelled: {OrderId}", orderId);

            return true;
        }

        public async Task<OrderResponse?> GetByIdAsync(Guid orderId)
        {
            // repo yang return IQueryable / include Payment+Items juga oke
            var order = await _orderRepository.GetByIdWithItemsAndPaymentAsNoTrackingAsync(orderId);
            if (order is null) return null;

            return _mapper.Map<OrderResponse>(order);
        }

        // =========================
        // Helpers (biar rapih kaya ProductService)
        // =========================

        private async Task<Domain.Entities.Payments.Payment> EnsureActivePaymentAsync(Guid paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment is null || !payment.IsActive)
                throw new Exception("Payment Method Not Found");

            return payment;
        }

        // private async Task<Domain.Entities.Carts.Cart> EnsureActiveCartAsync(Guid userId)
        // {
        //     var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
        //     if (cart is null || cart.Items.Count == 0)
        //         throw new Exception("Cart is empty");

        //     return cart;
        // }

        private async Task<Domain.Entities.Carts.Cart> EnsureCartForCheckoutAsync(Guid userId, Guid cartId)
        {
            var cart = await _cartRepository.GetByIdWithItemsAsync(cartId);
            if (cart is null)
                throw new Exception("Cart Not Found");

            if (cart.UserId != userId)
                throw new Exception("Forbidden: cart does not belong to user");

            if (cart.Items is null || cart.Items.Count == 0)
                throw new Exception("Cart is empty");

            return cart;
        }

        private async Task EnsureStockAvailableAsync(Guid productId, int requestedQty, DateTime now)
        {
            var p = await _productRepository.GetByIdForUpdateAsync(productId);
            if (p is null) throw new Exception("Product Not Found");

            var reserved = await _reservationRepository.GetReservedQtyAsync(productId, now);
            var available = p.Stock - reserved;

            if (available < requestedQty)
                throw new InvalidOperationException($"Insufficient stock. ProductId={productId}, Available={available}");
        }

        private static void EnsurePayable(Order order)
        {
            if (order.Status != "PendingPayment")
                throw new Exception("Order is not payable");
        }

        private static Order CreateOrderDraft(CheckoutRequest request, DateTime now, List<Domain.Entities.Carts.CartItem> items)
        {
            var order = new Order
            {
                UserId = request.UserId,
                PaymentId = request.PaymentId,
                Status = "PendingPayment",
                ExpiresAt = now.Add(PaymentTtl),
                CreatedAt = now
            };

            foreach (var ci in items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = ci.ProductId,
                    // ProductName = ci.Product?.Name ?? ci.ProductName, // fallback kalau cart item punya snapshot
                    ProductName = ci.Product.Name,
                    UnitPrice = ci.UnitPrice,
                    Quantity = ci.Quantity,
                    LineTotal = ci.UnitPrice * ci.Quantity
                });
            }

            order.TotalAmount = order.Items.Sum(x => x.LineTotal);
            return order;
        }
    }
}
