using Inventory.Api.DTOs.Suppliers;

namespace Inventory.Api.Services.Interfaces;

public interface ISupplierService
{
    Task<List<SupplierResponseDto>> GetAllAsync();

    Task<SupplierResponseDto?> GetByIdAsync(int supplierId);

    Task<SupplierResponseDto> CreateAsync(
        CreateSupplierRequestDto request);

    Task<SupplierResponseDto?> UpdateAsync(
        int supplierId,
        UpdateSupplierRequestDto request);

    Task<bool> DeleteAsync(int supplierId);
}
