using Application.Services.Categories;
using Shared.DTO.Categories;

namespace Api.Endpoints.Categories
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this WebApplication app)
        {
            var categoryGroup = app
                .MapGroup("/api/categories")
                .WithTags("Categories");

            // GET: /api/categories
            categoryGroup.MapGet("/", async (
                ICategoryService categoryService,
                int offset = 1,
                int limit = 10,
                string strQueryParam = "") =>
            {
                var result = await categoryService
                    .GetAllItems(offset, limit, strQueryParam);

                return Results.Ok(result);
            });

            // GET: /api/categories/{id}
            categoryGroup.MapGet("/{strUUID}", async (ICategoryService categoryService, Guid id) =>
            {
                var result = await categoryService.GetItemDetailById(id);
                return Results.Ok(result);
            });

            // POST: /api/categories
            categoryGroup.MapPost("/", async (CategoryCreateRequest request, ICategoryService service) =>
            {
                var result = await service.CreateAsync(request);
                return Results.Created($"/api/categories/{result.Id}", result);
            });

            // PUT: /api/categories/{id}
            categoryGroup.MapPut("/{id:guid}", async (Guid id, CategoryUpdateRequest request, ICategoryService service) =>
            {
                var updated = await service.UpdateAsync(id, request);
                return updated ? Results.NoContent() : Results.NotFound();
            });

            // DELETE: /api/categories/{id}
            categoryGroup.MapDelete("/{id:guid}", async (
                ICategoryService categoryService,
                Guid id) =>
            {
                var deleted = await categoryService
                    .DeleteAsync(id);

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            });
        }
    }
}
