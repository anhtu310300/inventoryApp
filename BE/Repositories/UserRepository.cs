using Inventory.Api.Data;
using Inventory.Api.Entities;
using Inventory.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> UpdateAsync(User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        if (existingUser == null)
        {
            return null;
        }

        existingUser.FullName = user.FullName;
        existingUser.Email = user.Email;
        existingUser.Phone = user.Phone;
        existingUser.Department = user.Department;
        existingUser.Role = user.Role;
        existingUser.Status = user.Status;
        existingUser.StartDate = user.StartDate;
        existingUser.Notes = user.Notes;

        await _context.SaveChangesAsync();

        return existingUser;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return true;
    }
}
