using Inventory.Api.Data;
using Inventory.Api.Domain;
using Inventory.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly InventoryDbContext _context;

    public SupplierRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers
            .OrderBy(supplier => supplier.Id)
            .ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int supplierId)
    {
        return await _context.Suppliers.FindAsync(supplierId);
    }

    public async Task<Supplier> CreateAsync(Supplier supplier)
    {
        await _context.Suppliers.AddAsync(supplier);

        await _context.SaveChangesAsync();

        return supplier;
    }

    public async Task<Supplier> UpdateAsync(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);

        await _context.SaveChangesAsync();

        return supplier;
    }

    public async Task<bool> DeleteAsync(int supplierId)
    {
        var supplier = await _context.Suppliers.FindAsync(supplierId);

        if (supplier is null)
        {
            return false;
        }

        supplier.Status = "Inactive";
        supplier.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
