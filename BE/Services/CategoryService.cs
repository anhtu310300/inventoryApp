using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Categories;
using Inventory.Api.Repositories.Interfaces;
using Inventory.Api.Services.Interfaces;

namespace Inventory.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();

        return _mapper.Map<List<CategoryResponseDto>>(categories);
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(
        int categoryId)
    {
        var category =
            await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
        {
            return null;
        }

        return _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task<CategoryResponseDto> CreateAsync(
        CreateCategoryRequestDto request)
    {
        var category = _mapper.Map<Category>(request);

        category.IsActive = true;
        category.CreatedAt = DateTime.UtcNow;

        var createdCategory =
            await _categoryRepository.CreateAsync(category);

        return _mapper.Map<CategoryResponseDto>(createdCategory);
    }

    public async Task<CategoryResponseDto?> UpdateAsync(
        int categoryId,
        UpdateCategoryRequestDto request)
    {
        var category =
            await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
        {
            return null;
        }

        _mapper.Map(request, category);

        var updatedCategory =
            await _categoryRepository.UpdateAsync(category);

        return _mapper.Map<CategoryResponseDto>(updatedCategory);
    }

    public async Task<bool> DeleteAsync(int categoryId)
    {
        return await _categoryRepository.DeleteAsync(categoryId);
    }
}
