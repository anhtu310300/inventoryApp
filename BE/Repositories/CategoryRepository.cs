using Inventory.Api.Data;
using Inventory.Api.Domain;
using Inventory.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly InventoryDbContext _context;

    public CategoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .OrderBy(category => category.Id)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int categoryId)
    {
        return await _context.Categories.FindAsync(categoryId);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await _context.Categories.AddAsync(category);

        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);

        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<bool> DeleteAsync(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);

        if (category is null)
        {
            return false;
        }

        category.IsActive = false;

        await _context.SaveChangesAsync();

        return true;
    }
}
