using Inventory.Api.Data;
using Inventory.Api.Domain;
using Inventory.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly InventoryDbContext _context;

    public CustomerRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .OrderBy(customer => customer.Id)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        return await _context.Customers.FindAsync(customerId);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);

        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);

        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<bool> DeleteAsync(int customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);

        if (customer is null)
        {
            return false;
        }

        _context.Customers.Remove(customer);

        await _context.SaveChangesAsync();

        return true;
    }
}
