using Inventory.Api.Domain;

namespace Inventory.Api.Repositories.Interfaces;

public interface IWarehouseRepository
{
    Task<List<Warehouse>> GetAllAsync();

    Task<Warehouse?> GetByIdAsync(int warehouseId);

    Task<Warehouse> CreateAsync(Warehouse warehouse);

    Task<Warehouse> UpdateAsync(Warehouse warehouse);

    Task<bool> DeleteAsync(int warehouseId);
}
