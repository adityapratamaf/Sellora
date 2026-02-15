namespace Shared.DTO.Orders;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public Guid PaymentId { get; set; }
    public string PaymentName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime ExpiresAt { get; set; }

    public List<OrderItemResponse> Items { get; set; } = new();
}
