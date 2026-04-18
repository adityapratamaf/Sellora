using Shared.DTO.Auth;

namespace Application.Services.Auth;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterRequest request);
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}