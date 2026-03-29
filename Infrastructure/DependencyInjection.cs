// using Domain.Interfaces.Users;
using Domain.Interfaces.Products;
using Infrastructure.Data;
// using Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces.Categories;
using Infrastructure.Repositories.Products;
using Infrastructure.Repositories.Categories;
using Domain.Interfaces.Payments;
using Infrastructure.Repositories.Payments;
using Domain.Interfaces.Carts;
using Infrastructure.Repositories.Carts;
using Infrastructure.Repositories.Orders;
using Domain.Interfaces.Orders;

using Infrastructure.Identity;
using Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Common.Models;
using Application.Services.Auth;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddHostedService<ExpiredReservationCleanupService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}

