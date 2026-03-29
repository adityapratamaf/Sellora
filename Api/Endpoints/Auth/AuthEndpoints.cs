using Application.Services.Auth;
using Shared.DTO.Auth;

namespace Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", async (IAuthService service, LoginRequest request) =>
        {
            var result = await service.LoginAsync(request);

            return result == null
                ? Results.Unauthorized()
                : Results.Ok(result);
        });

        group.MapPost("/logout", async (IAuthService service) =>
        {
            await service.LogoutAsync();
            return Results.Ok();
        })
        .RequireAuthorization();
    }
}