using Domain.Entities.Carts;
using Domain.Interfaces.Carts;
using Domain.Interfaces.Products;
using Microsoft.Extensions.Logging;
using Shared.DTO.Carts;

namespace Application.Services.Carts;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CartService> _log;

    public CartService(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IProductRepository productRepository,
        ILogger<CartService> log)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _log = log;
    }

    public async Task<CartResponse> GetOrCreateActiveCartAsync(Guid userId)
    {
        var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);

        if (cart is null)
        {
            cart = await _cartRepository.CreateAsync(new Cart
            {
                UserId = userId,
                IsActive = true
            });
        }

        // reload (biar items kebawa kalau create)
        var fresh = await _cartRepository.GetActiveCartByUserIdAsync(userId)
                    ?? throw new Exception("Failed to load cart");

        return MapToResponse(fresh);
    }

    public async Task<CartResponse> AddItemAsync(CartItemUpsertRequest request)
    {
        if (request.Quantity <= 0)
            throw new Exception("Quantity must be greater than 0");

        // pastikan cart aktif ada
        var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);
        if (cart is null)
            cart = await _cartRepository.CreateAsync(new Cart { UserId = request.UserId, IsActive = true });

        // ambil product untuk snapshot price
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            throw new Exception("Product Not Found");

        // cek item existing (upsert)
        var existingItem = await _cartItemRepository.GetByCartIdAndProductIdAsync(cart.Id, request.ProductId);

        if (existingItem is null)
        {
            await _cartItemRepository.CreateAsync(new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                UnitPrice = product.Price,
                Quantity = request.Quantity
            });
        }
        else
        {
            existingItem.Quantity += request.Quantity;
            existingItem.UnitPrice = product.Price; // optional: refresh price, atau biarkan snapshot pertama
            existingItem.UpdatedAt = DateTime.UtcNow;

            await _cartItemRepository.UpdateAsync(existingItem);
        }

        _log.LogInformation("Cart item upserted. UserId: {UserId}, ProductId: {ProductId}", request.UserId, request.ProductId);

        var fresh = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId)
                    ?? throw new Exception("Failed to load cart");

        return MapToResponse(fresh);
    }

    public async Task<bool> UpdateItemQtyAsync(Guid cartItemId, CartItemUpdateQtyRequest request)
    {
        if (request.Quantity <= 0)
            throw new Exception("Quantity must be greater than 0");

        var item = await _cartItemRepository.GetByIdAsync(cartItemId);
        if (item is null) return false;

        // NOTE: GetByIdAsync AsNoTracking -> but we update by sending new entity or implement tracked fetch.
        // Easiest: create entity with same id (repo Update uses Update()).
        var entity = new CartItem
        {
            Id = item.Id,
            CartId = item.CartId,
            ProductId = item.ProductId,
            UnitPrice = item.UnitPrice,
            Quantity = request.Quantity,
            CreatedAt = item.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        return await _cartItemRepository.UpdateAsync(entity);
    }

    public async Task<bool> RemoveItemAsync(Guid cartItemId)
    {
        return await _cartItemRepository.DeleteAsync(cartItemId);
    }

    public async Task<bool> ClearCartAsync(Guid userId)
    {
        var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
        if (cart is null) return true;

        return await _cartItemRepository.DeleteByCartIdAsync(cart.Id);
    }

    private static CartResponse MapToResponse(Domain.Entities.Carts.Cart cart)
    {
        var items = cart.Items.Select(i => new CartItemResponse
        {
            Id = i.Id,
            ProductId = i.ProductId,
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity
        }).ToList();

        return new CartResponse
        {
            Id = cart.Id,
            UserId = cart.UserId,
            IsActive = cart.IsActive,
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt,
            Items = items,
            TotalAmount = items.Sum(x => x.UnitPrice * x.Quantity)
        };
    }
}
