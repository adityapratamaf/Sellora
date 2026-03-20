namespace Shared.DTO.Orders;

public class CheckoutRequest
{
    public Guid UserId { get; set; }
    public Guid CartId { get; set; }
    public Guid PaymentId { get; set; }
}
