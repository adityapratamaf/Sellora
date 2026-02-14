namespace Shared.DTO.Carts;

public class CartResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<CartItemResponse> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
}
