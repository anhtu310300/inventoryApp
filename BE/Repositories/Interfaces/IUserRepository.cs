using Inventory.Api.Entities;

namespace Inventory.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}
