using Ecommerce.Core.DTOs;

namespace Ecommerce.Core.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(string token);
    Task LogoutAsync(int userId);
    Task<UserDto?> GetUserByIdAsync(int userId);
}
