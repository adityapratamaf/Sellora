using Domain.Entities.Carts;

namespace Domain.Interfaces.Carts;

public interface ICartRepository
{
    Task<Cart?> GetActiveCartByUserIdAsync(Guid userId);
    Task<Cart?> GetByIdAsync(Guid id);
    Task<Cart> CreateAsync(Cart cart);
    Task<bool> UpdateAsync(Cart cart);
}
