using Inventory.Api.Data;
using Inventory.Api.Domain;
using Inventory.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly InventoryDbContext _context;

    public AuthRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetByUserNameAsync(string userName)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(
                employee => employee.UserName == userName);
    }
}
