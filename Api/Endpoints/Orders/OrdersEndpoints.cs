using Application.Services.Orders;
using Shared.DTO.Orders;

namespace Api.Endpoints.Orders
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this WebApplication app)
        {
            var orderGroup = app.MapGroup("/api/orders").WithTags("Orders");

            // POST: /api/orders/checkout
            orderGroup.MapPost("/checkout", async (CheckoutRequest request, IOrderService orderService) =>
            {
                try
                {
                    var result = await orderService.CheckoutAsync(request);
                    return Results.Ok(result);
                }
                catch (InvalidOperationException ex)
                {
                    // stok tidak cukup / sedang reserved
                    return Results.Conflict(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            });

            // POST: /api/orders/{orderId}/confirm-payment
            orderGroup.MapPost("/{orderId:guid}/confirm-payment", async (Guid orderId, IOrderService orderService) =>
            {
                try
                {
                    var ok = await orderService.ConfirmPaymentAsync(orderId);
                    if (!ok) return Results.Conflict(new { message = "Order expired or cannot be paid" });

                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            });

            // POST: /api/orders/{orderId}/cancel
            orderGroup.MapPost("/{orderId:guid}/cancel", async (Guid orderId, IOrderService orderService) =>
            {
                try
                {
                    var ok = await orderService.CancelAsync(orderId);
                    if (!ok) return Results.NotFound();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            });

            // GET: /api/orders/{orderId}
            orderGroup.MapGet("/{orderId:guid}", async (Guid orderId, IOrderService orderService) =>
            {
                var result = await orderService.GetByIdAsync(orderId);
                return result is null ? Results.NotFound() : Results.Ok(result);
            });
        }
    }
}
