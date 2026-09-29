using Inventory.Api.DTOs;
using Inventory.Api.Entities;
using Inventory.Api.Repositories.Interfaces;
using Inventory.Api.Services.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace Inventory.Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public EmployeeService(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<EmployeeDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(MapToDto).ToList();
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return null;
        }

        return MapToDto(user);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var user = new User
        {
            EmployeeCode = dto.EmployeeCode,
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Department = dto.Department,
            Role = dto.Role,
            Status = dto.Status,
            StartDate = dto.StartDate,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            dto.Password
        );

        var createdUser =
            await _userRepository.CreateAsync(user);

        return MapToDto(createdUser);
    }

    public async Task<EmployeeDto?> UpdateAsync(
        int id,
        UpdateEmployeeDto dto)
    {
        var user = new User
        {
            Id = id,
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Department = dto.Department,
            Role = dto.Role,
            Status = dto.Status,
            StartDate = dto.StartDate,
            Notes = dto.Notes
        };

        var updatedUser =
            await _userRepository.UpdateAsync(user);

        if (updatedUser == null)
        {
            return null;
        }

        return MapToDto(updatedUser);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }

    private static EmployeeDto MapToDto(User user)
    {
        return new EmployeeDto
        {
            Id = user.Id,
            EmployeeCode = user.EmployeeCode,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Department = user.Department,
            Role = user.Role,
            Status = user.Status,
            StartDate = user.StartDate,
            Notes = user.Notes,
            CreatedAt = user.CreatedAt
        };
    }
}
