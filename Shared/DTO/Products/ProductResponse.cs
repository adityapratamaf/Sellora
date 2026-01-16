namespace Shared.DTO.Products;

public class ProductResponse
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }

    public string? CategoryName { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageProduct { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
