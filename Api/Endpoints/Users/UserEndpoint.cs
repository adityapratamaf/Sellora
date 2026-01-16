using Application.Services.User;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Users;

namespace Api.Endpoints;

public static class UserEndpoint
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users");

        group.MapGet("/", async (
            [FromServices] IUserService service,
            [FromQuery] int offset = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? q = null) =>
        {
            var result = await service.GetAllItems(offset, limit, q ?? "");
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            [FromServices] IUserService service) =>
        {
            var result = await service.GetItemDetailById(id);
            return Results.Ok(result);
        });

        group.MapPost("/", async (
            [FromBody] UserCreateRequest request,
            [FromServices] IUserService service) =>
        {
            var result = await service.CreateAsync(request);
            return Results.Created($"/api/users/{result.Id}", result);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UserUpdateRequest request,
            [FromServices] IUserService service) =>
        {
            var updated = await service.UpdateAsync(id, request);
            return updated ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            [FromServices] IUserService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}
