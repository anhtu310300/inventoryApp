using Inventory.Api.DTOs.Warehouses;

namespace Inventory.Api.Services.Interfaces;

public interface IWarehouseService
{
    Task<List<WarehouseResponseDto>> GetAllAsync();

    Task<WarehouseResponseDto?> GetByIdAsync(int warehouseId);

    Task<WarehouseResponseDto> CreateAsync(
        CreateWarehouseRequestDto request);

    Task<WarehouseResponseDto?> UpdateAsync(
        int warehouseId,
        UpdateWarehouseRequestDto request);

    Task<bool> DeleteAsync(int warehouseId);
}
