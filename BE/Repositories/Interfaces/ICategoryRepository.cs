using Inventory.Api.Domain;

namespace Inventory.Api.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int categoryId);

    Task<Category> CreateAsync(Category category);

    Task<Category> UpdateAsync(Category category);

    Task<bool> DeleteAsync(int categoryId);
}
