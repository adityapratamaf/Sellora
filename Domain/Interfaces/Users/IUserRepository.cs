using Domain.Entities.Users;

namespace Domain.Interfaces.Users;

public interface IUserRepository
{
    IQueryable<User> GetAllAsQueryable();
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CreateAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid id);
}