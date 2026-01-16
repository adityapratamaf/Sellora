namespace Shared.DTO.Payments;

public class PaymentUpdateRequest
{
    public string Name { get; set; } = default!;
    public string? ImageLogo { get; set; }
    public bool IsActive { get; set; }
}