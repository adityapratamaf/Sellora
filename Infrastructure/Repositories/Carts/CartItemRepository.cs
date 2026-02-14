using Domain.Entities.Carts;
using Domain.Interfaces.Carts;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.AppDbContext;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Carts;

public class CartItemRepository : ICartItemRepository
{
    private readonly AppDbContext _context;

    public CartItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CartItem?> GetByIdAsync(Guid id)
    {
        return await _context.CartItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CartItem?> GetByCartIdAndProductIdAsync(Guid cartId, Guid productId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId);
    }

    public async Task<CartItem> CreateAsync(CartItem item)
    {
        _context.CartItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateAsync(CartItem item)
    {
        _context.CartItems.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.CartItems.FirstOrDefaultAsync(x => x.Id == id);
        if (existing is null) return false;

        _context.CartItems.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteByCartIdAsync(Guid cartId)
    {
        var items = await _context.CartItems.Where(x => x.CartId == cartId).ToListAsync();
        if (items.Count == 0) return true;

        _context.CartItems.RemoveRange(items);
        await _context.SaveChangesAsync();
        return true;
    }
}
