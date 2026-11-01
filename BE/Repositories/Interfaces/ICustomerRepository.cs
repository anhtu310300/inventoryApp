using Inventory.Api.Domain;

namespace Inventory.Api.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();

    Task<Customer?> GetByIdAsync(int customerId);

    Task<Customer> CreateAsync(Customer customer);

    Task<Customer> UpdateAsync(Customer customer);

    Task<bool> DeleteAsync(int customerId);
}
