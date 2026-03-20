namespace Domain.Entities.Orders;

public class StockReservation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    // reservation valid sampai kapan
    public DateTime ReservedUntil { get; set; }

    public bool IsReleased { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
