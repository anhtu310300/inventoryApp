using Inventory.Api.Entities;


namespace Inventory.Api.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
