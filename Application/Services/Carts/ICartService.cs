using Shared.DTO.Carts;

namespace Application.Services.Carts;

public interface ICartService
{
    Task<CartResponse> GetOrCreateActiveCartAsync(Guid userId);

    Task<CartResponse> AddItemAsync(CartItemUpsertRequest request);
    Task<bool> UpdateItemQtyAsync(Guid cartItemId, CartItemUpdateQtyRequest request);
    Task<bool> RemoveItemAsync(Guid cartItemId);
    Task<bool> ClearCartAsync(Guid userId);
}
