using Application.Services.Auth;
using Microsoft.AspNetCore.Http;
using Shared.DTO.Auth;

namespace Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", async (HttpContext httpContext, IAuthService service, LoginRequest request) =>
        {
            var result = await service.LoginAsync(request);

            if (result == null)
                return Results.Unauthorized();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // todo : localhost development
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            httpContext.Response.Cookies.Append("access_token", result.Token, cookieOptions);

            return Results.Ok(new
            {
                message = "Login Success",
                name = result.Name,
                email = result.Email,
                role = result.Role,
                Token = result.Token
            });
        });

        group.MapPost("/logout", (HttpContext httpContext) =>
        {
            httpContext.Response.Cookies.Delete("access_token");

            return Results.Ok(new
            {
                message = "Logout Success"
            });
        })
        .RequireAuthorization();
    }
}