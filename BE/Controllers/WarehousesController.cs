using FluentValidation;
using FluentValidation.Results;
using Inventory.Api.Common.Responses;
using Inventory.Api.DTOs.Warehouses;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;
    private readonly IValidator<CreateWarehouseRequestDto>
        _createValidator;
    private readonly IValidator<UpdateWarehouseRequestDto>
        _updateValidator;

    public WarehousesController(
        IWarehouseService warehouseService,
        IValidator<CreateWarehouseRequestDto> createValidator,
        IValidator<UpdateWarehouseRequestDto> updateValidator)
    {
        _warehouseService = warehouseService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var warehouses = await _warehouseService.GetAllAsync();

        return Ok(new ApiResponse<List<WarehouseResponseDto>>
        {
            Data = warehouses,
            Messages = new List<string>
            {
                "Warehouses retrieved successfully."
            }
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);

        if (warehouse is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Warehouse was not found."
                }
            });
        }

        return Ok(new ApiResponse<WarehouseResponseDto>
        {
            Data = warehouse,
            Messages = new List<string>
            {
                "Warehouse retrieved successfully."
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateWarehouseRequestDto request)
    {
        var validationResult =
            await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var warehouse = await _warehouseService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = warehouse.Id },
            new ApiResponse<WarehouseResponseDto>
            {
                Data = warehouse,
                Messages = new List<string>
                {
                    "Warehouse created successfully."
                }
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateWarehouseRequestDto request)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var warehouse =
            await _warehouseService.UpdateAsync(id, request);

        if (warehouse is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Warehouse was not found."
                }
            });
        }

        return Ok(new ApiResponse<WarehouseResponseDto>
        {
            Data = warehouse,
            Messages = new List<string>
            {
                "Warehouse updated successfully."
            }
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _warehouseService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Warehouse was not found."
                }
            });
        }

        return Ok(new ApiResponse<object?>
        {
            Data = null,
            Messages = new List<string>
            {
                "Warehouse deactivated successfully."
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
