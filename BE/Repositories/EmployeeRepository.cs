using Inventory.Api.Data;
using Inventory.Api.Domain;
using Inventory.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly InventoryDbContext _context;

    public EmployeeRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .OrderBy(employee => employee.Id)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int employeeId)
    {
        return await _context.Employees.FindAsync(employeeId);
    }

    public async Task<Employee> CreateAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);

        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);

        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<bool> DeleteAsync(int employeeId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);

        if (employee is null)
        {
            return false;
        }

        employee.Status = "Inactive";
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
