using Domain.Entities.Categories;
using Domain.Entities.Users;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Payments;

namespace Infrastructure.Data.AppDbContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CATEGORY TABLE
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Description)
                .HasColumnName("description")
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
        });

        // USER TABLE
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Username)
                .HasColumnName("username")
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Password)
                .HasColumnName("password")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(x => x.Address)
                .HasColumnName("address")
                .HasMaxLength(255);

            entity.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(50);

            entity.Property(x => x.Role)
                .HasColumnName("role")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
            entity.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
        });

        // PRODUCT TABLE
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);
            
            entity.Property(x => x.ImageProduct)
                .HasColumnName("image_product")
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(x => x.Stock)
                .HasColumnName("stock")
                .IsRequired();

            entity.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            entity.Property(x => x.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // FK RELATIONSHIP
            entity.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PAYMENT TABLE
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("payments");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.ImageLogo)
                .HasColumnName("image_logo")
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
        }); 
    
    }
}