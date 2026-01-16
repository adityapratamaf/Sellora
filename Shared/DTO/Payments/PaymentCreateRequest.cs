namespace Shared.DTO.Payments;

public class PaymentCreateRequest
{
    public string Name { get; set; } = default!;
    public string? ImageLogo { get; set; }
    public bool IsActive { get; set; } = true;
}