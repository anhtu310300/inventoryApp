using Inventory.Api.Domain;

namespace Inventory.Api.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();

    Task<Employee?> GetByIdAsync(int employeeId);

    Task<Employee> CreateAsync(Employee employee);

    Task<Employee> UpdateAsync(Employee employee);

    Task<bool> DeleteAsync(int employeeId);
}
