using Inventory.Api.DTOs.Categories;

namespace Inventory.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryResponseDto>> GetAllAsync();

    Task<CategoryResponseDto?> GetByIdAsync(int categoryId);

    Task<CategoryResponseDto> CreateAsync(
        CreateCategoryRequestDto request);

    Task<CategoryResponseDto?> UpdateAsync(
        int categoryId,
        UpdateCategoryRequestDto request);

    Task<bool> DeleteAsync(int categoryId);
}
