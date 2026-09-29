using Inventory.Api.DTOs;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var registered = await _authService.RegisterAsync(dto);

            if (!registered)
            {
                return Conflict(new
                {
                    message = "Email này đã được sử dụng."
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = "Đăng ký tài khoản thành công." }
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Email hoặc mật khẩu không chính xác."
                });
            }

            return Ok(result);
        }
    }
}
