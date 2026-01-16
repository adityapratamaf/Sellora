using Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using AppDbContextType = Infrastructure.Data.AppDbContext.AppDbContext;

namespace Infrastructure.Data.Seed;

public static class CategorySeeder
{
    public static async Task SeedAsync(AppDbContextType context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var now = DateTime.UtcNow;

        context.Categories.AddRange(
            new Category
            {
                Name = "Electronic",
                Description = "Electronic",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Category
            {
                Name = "Fashion",
                Description = "Fashion",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Category
            {
                Name = "Home & Living",
                Description = "Home & Living",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            }
        );

        await context.SaveChangesAsync();
    }
}
