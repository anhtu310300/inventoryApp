using Inventory.Api.DTOs.Auth;

namespace Inventory.Api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
