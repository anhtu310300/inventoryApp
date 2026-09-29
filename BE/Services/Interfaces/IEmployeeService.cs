using Inventory.Api.DTOs;

namespace Inventory.Api.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task<bool> DeleteAsync(int id);
}