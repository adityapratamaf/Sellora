using System;
using Domain.Entities.Products;

namespace Domain.Entities.Carts;

public class CartItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    // Relasi Ke Product
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    // Snapshot Harga Saat Masuk Cart 
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
