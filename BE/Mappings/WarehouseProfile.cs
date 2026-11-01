using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Warehouses;

namespace Inventory.Api.Mappings;

public class WarehouseProfile : Profile
{
    public WarehouseProfile()
    {
        CreateMap<Warehouse, WarehouseResponseDto>();

        CreateMap<CreateWarehouseRequestDto, Warehouse>();

        CreateMap<UpdateWarehouseRequestDto, Warehouse>();
    }
}
