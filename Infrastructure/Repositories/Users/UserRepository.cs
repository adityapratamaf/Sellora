using Domain.Entities.Users;
using Domain.Interfaces.Users;
using Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IQueryable<User> GetAllAsQueryable()
    {
        return _context.Users.AsNoTracking();        
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var existing = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        if (existing is null) return false;

        existing.Name = user.Name;
        existing.Username = user.Username;
        existing.Email = user.Email;
        existing.Password = user.Password;
        existing.Address = user.Address;
        existing.Phone = user.Phone;
        existing.Role = user.Role;
        existing.IsActive = user.IsActive;
        existing.UpdatedAt = user.UpdatedAt;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (existing is null) return false;

        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}