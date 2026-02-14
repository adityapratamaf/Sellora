using Domain.Entities.Carts;

namespace Domain.Interfaces.Carts;

public interface ICartItemRepository
{
    Task<CartItem?> GetByIdAsync(Guid id);
    Task<CartItem?> GetByCartIdAndProductIdAsync(Guid cartId, Guid productId);

    Task<CartItem> CreateAsync(CartItem item);
    Task<bool> UpdateAsync(CartItem item);
    Task<bool> DeleteAsync(Guid id);

    Task<bool> DeleteByCartIdAsync(Guid cartId);
}
