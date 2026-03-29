using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppDbContextType = Infrastructure.Data.AppDbContext;

namespace Infrastructure.Data.Seed;

public static class UserSeeder
{
    public static async Task SeedAsync(
        AppDbContextType context,
        UserManager<ApplicationUser> userManager)
    {
        if (await context.Users.AnyAsync())
            return;

        var admin = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@test.com",
            Name = "Administrator"
        };

        await userManager.CreateAsync(admin, "Admin123!");
        await userManager.AddToRoleAsync(admin, "Admin");


        var customer = new ApplicationUser
        {
            UserName = "customer",
            Email = "customer@test.com",
            Name = "Customer"
        };

        await userManager.CreateAsync(customer, "Customer123!");
        await userManager.AddToRoleAsync(customer, "Customer");
    }
}