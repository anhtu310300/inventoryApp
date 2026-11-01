using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Suppliers;

namespace Inventory.Api.Mappings;

public class SupplierProfile : Profile
{
    public SupplierProfile()
    {
        CreateMap<Supplier, SupplierResponseDto>();

        CreateMap<CreateSupplierRequestDto, Supplier>();

        CreateMap<UpdateSupplierRequestDto, Supplier>();
    }
}
