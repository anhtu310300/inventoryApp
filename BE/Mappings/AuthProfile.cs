using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Auth;

namespace Inventory.Api.Mappings;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<Employee, LoginResponseDto>();
    }
}
