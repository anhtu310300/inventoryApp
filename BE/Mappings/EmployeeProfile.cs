using AutoMapper;
using Inventory.Api.Domain;
using Inventory.Api.DTOs.Employees;

namespace Inventory.Api.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeResponseDto>();

        CreateMap<CreateEmployeeRequestDto, Employee>();

        CreateMap<UpdateEmployeeRequestDto, Employee>();
    }
}
