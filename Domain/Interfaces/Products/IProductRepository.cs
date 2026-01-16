using Domain.Entities.Products;

namespace Domain.Interfaces.Products;

public interface IProductRepository
{
    IQueryable<Product> GetAllAsQueryable();
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
}
