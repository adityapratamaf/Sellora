using Domain.Interfaces.Users;
using Domain.Interfaces.Products;
using Infrastructure.Data.AppDbContext;
using Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces.Categories;
using Infrastructure.Repositories.Products;
using Infrastructure.Repositories.Categories;
using Domain.Interfaces.Payments;
using Infrastructure.Repositories.Payments;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        return services;
    }
}
