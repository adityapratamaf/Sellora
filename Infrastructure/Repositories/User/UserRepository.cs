using Domain.Interfaces.Users;
using Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using UserEntity = Domain.Entities.Users.User;

namespace Infrastructure.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<UserEntity> GetAllAsQueryable()
        => _context.Users.AsNoTracking().AsQueryable();

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
        => await _context.Users.AsNoTracking().ToListAsync();

    public async Task<UserEntity?> GetByIdAsync(Guid id)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(UserEntity user)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
        if (existing is null) return false;

        existing.Name = user.Name;
        existing.Username = user.Username;
        existing.Email = user.Email;
        existing.Password = user.Password;
        existing.Address = user.Address;
        existing.Phone = user.Phone;
        existing.Role = user.Role;
        existing.IsActive = user.IsActive;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (existing is null) return false;

        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
