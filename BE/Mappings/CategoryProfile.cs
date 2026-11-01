using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Categories;

namespace Inventory.Api.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponseDto>();

        CreateMap<CreateCategoryRequestDto, Category>();

        CreateMap<UpdateCategoryRequestDto, Category>();
    }
}
