using Inventory.Api.Domain;

namespace Inventory.Api.Repositories.Interfaces;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllAsync();

    Task<Supplier?> GetByIdAsync(int supplierId);

    Task<Supplier> CreateAsync(Supplier supplier);

    Task<Supplier> UpdateAsync(Supplier supplier);

    Task<bool> DeleteAsync(int supplierId);
}
