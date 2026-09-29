using Inventory.Api.DTOs;

namespace Inventory.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDto dto);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
    }
}
