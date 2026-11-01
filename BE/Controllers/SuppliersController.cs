using FluentValidation;
using FluentValidation.Results;
using Inventory.Api.Common.Responses;
using Inventory.Api.DTOs.Suppliers;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    private readonly IValidator<CreateSupplierRequestDto>
        _createValidator;
    private readonly IValidator<UpdateSupplierRequestDto>
        _updateValidator;

    public SuppliersController(
        ISupplierService supplierService,
        IValidator<CreateSupplierRequestDto> createValidator,
        IValidator<UpdateSupplierRequestDto> updateValidator)
    {
        _supplierService = supplierService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _supplierService.GetAllAsync();

        return Ok(new ApiResponse<List<SupplierResponseDto>>
        {
            Data = suppliers,
            Messages = new List<string>
            {
                "Suppliers retrieved successfully."
            }
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);

        if (supplier is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Supplier was not found."
                }
            });
        }

        return Ok(new ApiResponse<SupplierResponseDto>
        {
            Data = supplier,
            Messages = new List<string>
            {
                "Supplier retrieved successfully."
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSupplierRequestDto request)
    {
        var validationResult =
            await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var supplier = await _supplierService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = supplier.Id },
            new ApiResponse<SupplierResponseDto>
            {
                Data = supplier,
                Messages = new List<string>
                {
                    "Supplier created successfully."
                }
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateSupplierRequestDto request)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var supplier =
            await _supplierService.UpdateAsync(id, request);

        if (supplier is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Supplier was not found."
                }
            });
        }

        return Ok(new ApiResponse<SupplierResponseDto>
        {
            Data = supplier,
            Messages = new List<string>
            {
                "Supplier updated successfully."
            }
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _supplierService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Supplier was not found."
                }
            });
        }

        return Ok(new ApiResponse<object?>
        {
            Data = null,
            Messages = new List<string>
            {
                "Supplier deactivated successfully."
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
