using FluentValidation;
using Inventory.Api.Common.Responses;
using Inventory.Api.DTOs.Auth;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginRequestDto> _loginValidator;

    public AuthController(
        IAuthService authService,
        IValidator<LoginRequestDto> loginValidator)
    {
        _authService = authService;
        _loginValidator = loginValidator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request)
    {
        var validationResult =
            await _loginValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var messages = validationResult.Errors
                .Select(error => error.ErrorMessage)
                .Distinct()
                .ToList();

            return BadRequest(new ApiResponse<object?>
            {
                Data = null,
                Messages = messages
            });
        }

        try
        {
            var loginResponse =
                await _authService.LoginAsync(request);

            return Ok(new ApiResponse<LoginResponseDto>
            {
                Data = loginResponse,
                Messages = new List<string>
                {
                    "Login successful."
                }
            });
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    exception.Message
                }
            });
        }
    }
}
