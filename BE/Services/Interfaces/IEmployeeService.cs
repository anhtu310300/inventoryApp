using Inventory.Api.DTOs.Employees;

namespace Inventory.Api.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeResponseDto>> GetAllAsync();

    Task<EmployeeResponseDto?> GetByIdAsync(int employeeId);

    Task<EmployeeResponseDto> CreateAsync(
        CreateEmployeeRequestDto request);

    Task<EmployeeResponseDto?> UpdateAsync(
        int employeeId,
        UpdateEmployeeRequestDto request);

    Task<bool> DeleteAsync(int employeeId);
}
