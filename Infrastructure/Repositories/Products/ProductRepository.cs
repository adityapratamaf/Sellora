using Domain.Entities.Products;
using Domain.Interfaces.Products;
using Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<Product> GetAllAsQueryable()
        {
            return _context.Products.AsNoTracking();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var existing = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == product.Id);

            if (existing is null) return false;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.ImageProduct = product.ImageProduct;
            existing.Price = product.Price; 
            existing.Stock = product.Stock;
            existing.IsActive = product.IsActive;
            existing.CategoryId = product.CategoryId;
            existing.Category = product.Category;
            existing.UpdatedAt = product.UpdatedAt;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing is null) return false;

            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}