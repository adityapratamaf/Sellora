using System;

namespace Domain.Entities.Carts;

public class CartItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    // Relasi ke Product (pakai Domain.Entities.Products.Product di project kamu)
    public Guid ProductId { get; set; }

    // Snapshot harga saat masuk cart (penting)
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
