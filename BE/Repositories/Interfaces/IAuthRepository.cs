using Inventory.Api.Domain;

namespace Inventory.Api.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<Employee?> GetByUserNameAsync(string userName);
}
