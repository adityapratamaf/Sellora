using Application.Services.Carts;
using Shared.DTO.Carts;

namespace Api.Endpoints.Carts
{
    public static class CartEndpoints
    {
        public static void MapCartEndpoints(this WebApplication app)
        {
            var cartGroup = app.MapGroup("/api/cart").WithTags("Cart");

            // GET: /api/cart/{userId}
            cartGroup.MapGet("/{userId:guid}", async (ICartService cartService, Guid userId) =>
            {
                var cart = await cartService.GetOrCreateActiveCartAsync(userId);
                return Results.Ok(cart);
            });

            // POST: /api/cart/items
            cartGroup.MapPost("/items", async (CartItemUpsertRequest request, ICartService cartService) =>
            {
                var cart = await cartService.AddItemAsync(request);
                return Results.Ok(cart);
            });

            // PUT: /api/cart/items/{cartItemId}/qty
            cartGroup.MapPut("/items/{cartItemId:guid}/qty", async (Guid cartItemId, CartItemUpdateQtyRequest request, ICartService cartService) =>
            {
                var ok = await cartService.UpdateItemQtyAsync(cartItemId, request);
                return ok ? Results.NoContent() : Results.NotFound();
            });

            // DELETE: /api/cart/items/{cartItemId}
            cartGroup.MapDelete("/items/{cartItemId:guid}", async (ICartService cartService, Guid cartItemId) =>
            {
                var ok = await cartService.RemoveItemAsync(cartItemId);
                return ok ? Results.NoContent() : Results.NotFound();
            });

            // DELETE: /api/cart/clear/{userId}
            cartGroup.MapDelete("/clear/{userId:guid}", async (ICartService cartService, Guid userId) =>
            {
                await cartService.ClearCartAsync(userId);
                return Results.NoContent();
            });
        }
    }
}
