using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Suppliers;
using Inventory.Api.Repositories.Interfaces;
using Inventory.Api.Services.Interfaces;

namespace Inventory.Api.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;

    public SupplierService(
        ISupplierRepository supplierRepository,
        IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public async Task<List<SupplierResponseDto>> GetAllAsync()
    {
        var suppliers = await _supplierRepository.GetAllAsync();

        return _mapper.Map<List<SupplierResponseDto>>(suppliers);
    }

    public async Task<SupplierResponseDto?> GetByIdAsync(
        int supplierId)
    {
        var supplier =
            await _supplierRepository.GetByIdAsync(supplierId);

        if (supplier is null)
        {
            return null;
        }

        return _mapper.Map<SupplierResponseDto>(supplier);
    }

    public async Task<SupplierResponseDto> CreateAsync(
        CreateSupplierRequestDto request)
    {
        var supplier = _mapper.Map<Supplier>(request);

        supplier.Status = "Active";
        supplier.CreatedAt = DateTime.UtcNow;
        supplier.UpdatedAt = DateTime.UtcNow;

        var createdSupplier =
            await _supplierRepository.CreateAsync(supplier);

        return _mapper.Map<SupplierResponseDto>(createdSupplier);
    }

    public async Task<SupplierResponseDto?> UpdateAsync(
        int supplierId,
        UpdateSupplierRequestDto request)
    {
        var supplier =
            await _supplierRepository.GetByIdAsync(supplierId);

        if (supplier is null)
        {
            return null;
        }

        _mapper.Map(request, supplier);
        supplier.UpdatedAt = DateTime.UtcNow;

        var updatedSupplier =
            await _supplierRepository.UpdateAsync(supplier);

        return _mapper.Map<SupplierResponseDto>(updatedSupplier);
    }

    public async Task<bool> DeleteAsync(int supplierId)
    {
        return await _supplierRepository.DeleteAsync(supplierId);
    }
}
