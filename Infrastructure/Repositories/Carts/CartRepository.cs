using Domain.Entities.Carts;
using Domain.Interfaces.Carts;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.AppDbContext;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Carts;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetActiveCartByUserIdAsync(Guid userId)
    {
        return await _context.Carts
            .Include(x => x.Items)
            .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);
    }

    public async Task<Cart?> GetByIdAsync(Guid id)
    {
        return await _context.Carts
            .Include(x => x.Items)
            .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<bool> UpdateAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
        return true;
    }
}
