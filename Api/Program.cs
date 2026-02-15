using Api.EndpointMapper;
using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using AppDbContextType = Infrastructure.Data.AppDbContext.AppDbContext;
using Infrastructure.Data.Seed;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Sellora", 
        Version = "1.0" 
    });
});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

        app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Sellora API Docs")
            .WithTheme(ScalarTheme.Mars)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
    });
}

// Apply Migration (Seeding done by HasData)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContextType>();
    await db.Database.MigrateAsync();
    await UserSeeder.SeedAsync(db);
    await CategorySeeder.SeedAsync(db);
    await ProductSeeder.SeedAsync(db);
    await PaymentSeeder.SeedAsync(db);
}

app.UseStaticFiles();

// app.UseHttpsRedirection();
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.MapAllEndpoints();
app.Run();
