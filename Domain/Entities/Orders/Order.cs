using Domain.Entities.Payments;

namespace Domain.Entities.Orders;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    // Payment method yang dipilih user
    public Guid PaymentId { get; set; }
    public Payment Payment { get; set; } = default!;

    // PendingPayment | Paid | Cancelled | Expired
    public string Status { get; set; } = "PendingPayment";

    public decimal TotalAmount { get; set; }

    // batas waktu bayar
    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
