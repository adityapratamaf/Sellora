namespace Domain.Entities.Orders;

public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public Guid ProductId { get; set; }

    // Snapshot untuk histori order
    public string ProductName { get; set; } = default!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public decimal LineTotal { get; set; }
}
