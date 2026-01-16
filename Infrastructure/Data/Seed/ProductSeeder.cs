using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using AppDbContextType = Infrastructure.Data.AppDbContext.AppDbContext;

namespace Infrastructure.Data.Seed;

public static class ProductSeeder
{
    public static async Task SeedAsync(AppDbContextType context)
    {
        if (await context.Products.AnyAsync())
            return;

        var now = DateTime.UtcNow;

        var electronicCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Electronic");
        var fashionCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Fashion");
        var homeLivingCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Home & Living");

        if (electronicCategory is null || fashionCategory is null || homeLivingCategory is null)
            return;

        context.Products.AddRange(
            new Product
            {
                Name = "Smartphone",
                Description = "Latest model smartphone with advanced features.",
                ImageProduct = "/images/products/smartphone.png",
                Price = 699000,
                Stock = 50,
                IsActive = true,
                CategoryId = electronicCategory.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                Name = "Jeans",
                Description = "Comfortable and stylish denim jeans.",
                ImageProduct = "/images/products/jeans.png",
                Price = 49000,
                Stock = 100,
                IsActive = true,
                CategoryId = fashionCategory.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                Name = "Sofa",
                Description = "Modern sofa for your living room.",
                ImageProduct = "/images/products/sofa.png",
                Price = 899000,
                Stock = 20,
                IsActive = false,
                CategoryId = homeLivingCategory.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                Name = "Laptop",
                Description = "High-performance laptop for work and gaming.",
                ImageProduct = "/images/products/laptop.png",
                Price = 129000,
                Stock = 2,
                IsActive = true,
                CategoryId = electronicCategory.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                Name = "T-Shirt",
                Description = "Casual cotton t-shirt available in various colors.",
                ImageProduct = "/images/products/tshirt.png",
                Price = 190000,
                Stock = 200,
                IsActive = true,
                CategoryId = fashionCategory.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                Name = "Dining Table",
                Description = "Elegant dining table for family meals.",
                ImageProduct = "/images/products/dining-table.png",
                Price = 599000,
                Stock = 15,
                IsActive = false,
                CategoryId = homeLivingCategory.Id,
                CreatedAt = now,
                UpdatedAt = now
            }
        );

        await context.SaveChangesAsync();
    }
}