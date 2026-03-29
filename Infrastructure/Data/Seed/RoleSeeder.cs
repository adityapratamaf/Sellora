using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Seed;

public static class RoleSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));

        if (!await roleManager.RoleExistsAsync("Customer"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("Customer"));
    }
}