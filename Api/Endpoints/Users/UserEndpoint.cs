using Application.Services.User;
using Shared.DTO.Users;

namespace Api.Endpoints.Users
{
    public static class UserEndpoint
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            var userGroup = app.MapGroup("/api/users").WithTags("Users");

            // GET: /api/users
            userGroup.MapGet("/", async (IUserService userService, int offset = 1, int limit = 10, string strQueryParam = "") =>
            {
                var result = await userService.GetAllItems(offset, limit, strQueryParam);
                return Results.Ok(result);
            });

            // GET: /api/users/{id}
            userGroup.MapGet("/{strUUID}", async (IUserService userService, Guid id) =>
            {
                var result = await userService.GetItemDetailById(id);
                return Results.Ok(result);
            });

            // POST: /api/users
            userGroup.MapPost("/", async (UserCreateRequest request, IUserService service) =>
            {
                var result = await service.CreateAsync(request);
                return Results.Created($"/api/users/{result.Id}", result);
            });

            // PUT: /api/users/{id}
            userGroup.MapPut("/{id:guid}", async (Guid id, UserUpdateRequest request, IUserService service) =>
            {
                var updated = await service.UpdateAsync(id, request);
                return Results.Ok(updated);
            });

            // DELETE: /api/users/{id}
            userGroup.MapDelete("/{id:guid}", async (IUserService userService, Guid id) =>
            {
                var deleted = await userService.DeleteAsync(id);
                return Results.Ok(deleted);
            });
        }
    }
}