namespace Shared.DTO.Categories;

public class CategoryCreateRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
