using Application.Services.Products;
using Shared.DTO.Products;

namespace Api.Endpoints.Products;

public static class ProductEndpoint
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var productGroup = app.MapGroup("/api/products").WithTags("Products");

        // GET: /api/products
        productGroup.MapGet("/", async (IProductService productService, int offset = 1, int limit = 10, string strQueryParam = "") =>
        {
            var result = await productService.GetAllItems(offset, limit, strQueryParam);
            return Results.Ok(result);
        });

        // GET: /api/products/{id}
        productGroup.MapGet("/{strUUID}", async (IProductService productService, Guid id) =>
        {
            var result = await productService.GetItemDetailById(id);
            return Results.Ok(result);            
        });

        // POST: /api/products
        productGroup.MapPost("/", async (ProductCreateRequest request, IProductService service) =>
        {
            var result = await service.CreateAsync(request);
            return Results.Created($"/api/products/{result.Id}", result);
        });

        // PUT: /api/payments/{id}
        productGroup.MapPut("/{id:guid}", async (Guid id, ProductUpdateRequest request, IProductService service) =>
        {
            var updated = await service.UpdateAsync(id, request);
            return Results.Ok(updated);
        });

        // DELETE: /api/products/{id}
        productGroup.MapDelete("/{id:guid}", async (IProductService productService, Guid id) =>
        {
            var deleted = await productService.DeleteAsync(id);
            return Results.Ok(deleted);
        });

        // GET: /api/products/category
        productGroup.MapGet("/category", async (IProductService productService, Guid id, int offset = 1, int limit = 10, string strQueryParam = "") =>
        {
            var result = await productService.GetProductByCategory(id, offset, limit, strQueryParam);
            return Results.Ok(result);            
        });
    }
}
