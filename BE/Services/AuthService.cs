using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Auth;
using Inventory.Api.Repositories.Interfaces;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Api.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<Employee> _passwordHasher;
    private readonly IMapper _mapper;

    public AuthService(
        IAuthRepository authRepository,
        IPasswordHasher<Employee> passwordHasher,
        IMapper mapper)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto request)
    {
        var userName = request.UserName.Trim();

        var employee =
            await _authRepository.GetByUserNameAsync(userName);

        if (employee is null ||
            string.IsNullOrWhiteSpace(employee.PasswordHash))
        {
            throw new UnauthorizedAccessException(
                "Invalid username or password.");
        }

        var passwordResult =
            _passwordHasher.VerifyHashedPassword(
                employee,
                employee.PasswordHash,
                request.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException(
                "Invalid username or password.");
        }

        if (!string.Equals(
                employee.Status,
                "Active",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException(
                "The account is inactive.");
        }

        return _mapper.Map<LoginResponseDto>(employee);
    }
}
