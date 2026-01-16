using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using AppDbContextType = Infrastructure.Data.AppDbContext.AppDbContext;

namespace Infrastructure.Data.Seed;

public static class PaymentSeeder
{
    public static async Task SeedAsync(AppDbContextType context)
    {
        if (await context.Payments.AnyAsync())
            return;

        var now = DateTime.UtcNow;

        context.Payments.AddRange(
            new Payment
            {
                Name = "BNI",
                ImageLogo = "/images/payments/bni.png",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Payment
            {
                Name = "Mandiri",
                ImageLogo = "/images/payments/mandiri.png",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Payment
            {
                Name = "OVO",
                ImageLogo = "/images/payments/ovo.png",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Payment
            {
                Name = "GoPay",
                ImageLogo = "/images/payments/gopay.png",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            }
        );

        await context.SaveChangesAsync();
    }
}