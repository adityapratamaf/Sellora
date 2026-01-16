namespace Shared.DTO.Payments;

public class PaymentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? ImageLogo { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}