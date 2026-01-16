using Domain.Entities.Categories;

namespace Domain.Interfaces.Categories;

public interface ICategoryRepository
{
    IQueryable<Category> GetAllAsQueryable();
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category> CreateAsync(Category category);
    Task<bool> UpdateAsync(Category category);
    Task<bool> DeleteAsync(Guid id);
}
