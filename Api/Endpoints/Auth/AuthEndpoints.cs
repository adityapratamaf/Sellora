using System.Security.Claims;
using Application.Services.Auth;
using Microsoft.AspNetCore.Http;
using Shared.DTO.Auth;

namespace Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async (IAuthService service, RegisterRequest request) =>
        {
            try
            {
                var message = await service.RegisterAsync(request);
                return Results.Ok(new
                {
                    message
                });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new
                {
                    message = ex.Message
                });
            }
        });

        group.MapPost("/login", async (HttpContext httpContext, IAuthService service, LoginRequest request) =>
        {
            try
            {
                var result = await service.LoginAsync(request);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // todo : localhost development
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                };
            
                if (string.IsNullOrEmpty(result?.Token))
                    return Results.BadRequest(new
                    {
                        message = "Token Not Valid"
                    });

                httpContext.Response.Cookies.Append("access_token", result.Token, cookieOptions);

                return Results.Ok(new
                {
                    message = "Login Success",
                    name = result.Name,
                    email = result.Email,
                    role = result.Role,
                    Token = result.Token
                });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new
                {
                    message = ex.Message
                });
            }
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

        group.MapGet("/me", (ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var name = user.FindFirst(ClaimTypes.Name)?.Value;
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            return Results.Ok(new
            {
                id = userId,
                name,
                email,
                role
            });
        })
        .RequireAuthorization();
    }
}