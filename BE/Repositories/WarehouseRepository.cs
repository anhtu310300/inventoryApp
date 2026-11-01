using Inventory.Api.Data;
using Inventory.Api.Domain;
using Inventory.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly InventoryDbContext _context;

    public WarehouseRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Warehouse>> GetAllAsync()
    {
        return await _context.Warehouses
            .OrderBy(warehouse => warehouse.Id)
            .ToListAsync();
    }

    public async Task<Warehouse?> GetByIdAsync(int warehouseId)
    {
        return await _context.Warehouses.FindAsync(warehouseId);
    }

    public async Task<Warehouse> CreateAsync(Warehouse warehouse)
    {
        await _context.Warehouses.AddAsync(warehouse);

        await _context.SaveChangesAsync();

        return warehouse;
    }

    public async Task<Warehouse> UpdateAsync(Warehouse warehouse)
    {
        _context.Warehouses.Update(warehouse);

        await _context.SaveChangesAsync();

        return warehouse;
    }

    public async Task<bool> DeleteAsync(int warehouseId)
    {
        var warehouse =
            await _context.Warehouses.FindAsync(warehouseId);

        if (warehouse is null)
        {
            return false;
        }

        warehouse.Status = "Inactive";

        await _context.SaveChangesAsync();

        return true;
    }
}
