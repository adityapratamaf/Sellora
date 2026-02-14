namespace Shared.DTO.Carts;

public class CartItemUpsertRequest
{
    public Guid UserId { get; set; }       // sementara pakai dari request (nanti dari auth claim)
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
