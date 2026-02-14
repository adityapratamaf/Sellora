using System;
using System.Collections.Generic;

namespace Domain.Entities.Carts;

public class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Owner cart (bisa dari auth user)
    public Guid UserId { get; set; }

    // Cart aktif atau sudah checkout (nanti)
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
