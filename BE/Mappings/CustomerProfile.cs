using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Customers;

namespace Inventory.Api.Mappings;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerResponseDto>();

        CreateMap<CreateCustomerRequestDto, Customer>();

        CreateMap<UpdateCustomerRequestDto, Customer>();
    }
}
