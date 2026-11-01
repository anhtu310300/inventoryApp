using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Employees;
using Inventory.Api.Repositories.Interfaces;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPasswordHasher<Employee> _passwordHasher;
    private readonly IMapper _mapper;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IPasswordHasher<Employee> passwordHasher,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<List<EmployeeResponseDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return _mapper.Map<List<EmployeeResponseDto>>(employees);
    }

    public async Task<EmployeeResponseDto?> GetByIdAsync(
        int employeeId)
    {
        var employee =
            await _employeeRepository.GetByIdAsync(employeeId);

        if (employee is null)
        {
            return null;
        }

        return _mapper.Map<EmployeeResponseDto>(employee);
    }

    public async Task<EmployeeResponseDto> CreateAsync(
        CreateEmployeeRequestDto request)
    {
        var employee = _mapper.Map<Employee>(request);

        employee.Status = "Active";
        employee.CreatedAt = DateTime.UtcNow;
        employee.UpdatedAt = DateTime.UtcNow;

        employee.PasswordHash =
            _passwordHasher.HashPassword(
                employee,
                request.Password);

        var createdEmployee =
            await _employeeRepository.CreateAsync(employee);

        return _mapper.Map<EmployeeResponseDto>(createdEmployee);
    }

    public async Task<EmployeeResponseDto?> UpdateAsync(
        int employeeId,
        UpdateEmployeeRequestDto request)
    {
        var employee =
            await _employeeRepository.GetByIdAsync(employeeId);

        if (employee is null)
        {
            return null;
        }

        _mapper.Map(request, employee);

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            employee.PasswordHash =
                _passwordHasher.HashPassword(
                    employee,
                    request.Password);
        }

        employee.UpdatedAt = DateTime.UtcNow;

        var updatedEmployee =
            await _employeeRepository.UpdateAsync(employee);

        return _mapper.Map<EmployeeResponseDto>(updatedEmployee);
    }

    public async Task<bool> DeleteAsync(int employeeId)
    {
        return await _employeeRepository.DeleteAsync(employeeId);
    }
}
