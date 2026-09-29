using Inventory.Api.DTOs;
using Inventory.Api.Entities;
using Inventory.Api.Repositories.Interfaces;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto dto)
        {
            var email = dto.Email.Trim();

            if (await _userRepository.EmailExistsAsync(email))
            {
                return false;
            }

            var user = new User
            {
                EmployeeCode = $"EMP{Guid.NewGuid():N}"[..11].ToUpperInvariant(),
                FullName = dto.FullName.Trim(),
                Email = email,
                Phone = dto.Phone?.Trim(),
                Department = dto.Department.Trim(),
                Role = "Employee",
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                dto.Password
            );

            await _userRepository.CreateAsync(user);

            return true;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var email = dto.Email.Trim();
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                return null;
            }
            if(string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return null;
            }
            if (user.Status != "Active")
            {
                return null;
            }
            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return null;
            }
            var token = _tokenService.CreateToken(user);

            return new LoginResponseDto
            {
                AccessToken = token,
                UserId = user.Id,
                EmployeeCode = user.EmployeeCode,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };

        }

    
    }
}
