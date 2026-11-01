using FluentValidation;
using FluentValidation.Results;
using Inventory.Api.Common.Responses;
using Inventory.Api.DTOs.Employees;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IValidator<CreateEmployeeRequestDto>
        _createValidator;
    private readonly IValidator<UpdateEmployeeRequestDto>
        _updateValidator;

    public EmployeesController(
        IEmployeeService employeeService,
        IValidator<CreateEmployeeRequestDto> createValidator,
        IValidator<UpdateEmployeeRequestDto> updateValidator)
    {
        _employeeService = employeeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();

        return Ok(new ApiResponse<List<EmployeeResponseDto>>
        {
            Data = employees,
            Messages = new List<string>
            {
                "Employees retrieved successfully."
            }
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Employee was not found."
                }
            });
        }

        return Ok(new ApiResponse<EmployeeResponseDto>
        {
            Data = employee,
            Messages = new List<string>
            {
                "Employee retrieved successfully."
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeRequestDto request)
    {
        var validationResult =
            await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var employee =
            await _employeeService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            new ApiResponse<EmployeeResponseDto>
            {
                Data = employee,
                Messages = new List<string>
                {
                    "Employee created successfully."
                }
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateEmployeeRequestDto request)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var employee =
            await _employeeService.UpdateAsync(id, request);

        if (employee is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Employee was not found."
                }
            });
        }

        return Ok(new ApiResponse<EmployeeResponseDto>
        {
            Data = employee,
            Messages = new List<string>
            {
                "Employee updated successfully."
            }
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _employeeService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Employee was not found."
                }
            });
        }

        return Ok(new ApiResponse<object?>
        {
            Data = null,
            Messages = new List<string>
            {
                "Employee deactivated successfully."
            }
        });
    }

    private static IActionResult ValidationFailure(
        ValidationResult validationResult)
    {
        var messages = validationResult.Errors
            .Select(error => error.ErrorMessage)
            .Distinct()
            .ToList();

        return new BadRequestObjectResult(
            new ApiResponse<object?>
            {
                Data = null,
                Messages = messages
            });
    }
}
