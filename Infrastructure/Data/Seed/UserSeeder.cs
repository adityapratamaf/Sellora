using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using AppDbContextType = Infrastructure.Data.AppDbContext.AppDbContext;

namespace Infrastructure.Data.Seed;

public static class UserSeeder
{
    public static async Task SeedAsync(AppDbContextType context)
    {
        if (await context.Users.AnyAsync())
            return;

        var now = DateTime.UtcNow;

        context.Users.Add(new User
        {
            Name = "Admin",
            Username = "admin",
            Email = "admin@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("admin"),
            Address = "Jakarta",
            Phone = "08976641818",
            Role = "Admin",
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        });
        
        context.Users.Add(new User
        {
            Name = "Customer",
            Username = "customer",
            Email = "customer@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("customer"),
            Address = "Jakarta",
            Phone = "08976641818",
            Role = "Customer",
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        });

        await context.SaveChangesAsync();
    }
}
