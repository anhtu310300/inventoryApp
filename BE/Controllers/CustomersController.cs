using FluentValidation;
using FluentValidation.Results;
using Inventory.Api.Common.Responses;
using Inventory.Api.DTOs.Customers;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IValidator<CreateCustomerRequestDto>
        _createValidator;
    private readonly IValidator<UpdateCustomerRequestDto>
        _updateValidator;

    public CustomersController(
        ICustomerService customerService,
        IValidator<CreateCustomerRequestDto> createValidator,
        IValidator<UpdateCustomerRequestDto> updateValidator)
    {
        _customerService = customerService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();

        return Ok(new ApiResponse<List<CustomerResponseDto>>
        {
            Data = customers,
            Messages = new List<string>
            {
                "Customers retrieved successfully."
            }
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Customer was not found."
                }
            });
        }

        return Ok(new ApiResponse<CustomerResponseDto>
        {
            Data = customer,
            Messages = new List<string>
            {
                "Customer retrieved successfully."
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCustomerRequestDto request)
    {
        var validationResult =
            await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var customer = await _customerService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.Id },
            new ApiResponse<CustomerResponseDto>
            {
                Data = customer,
                Messages = new List<string>
                {
                    "Customer created successfully."
                }
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCustomerRequestDto request)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var customer =
            await _customerService.UpdateAsync(id, request);

        if (customer is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Customer was not found."
                }
            });
        }

        return Ok(new ApiResponse<CustomerResponseDto>
        {
            Data = customer,
            Messages = new List<string>
            {
                "Customer updated successfully."
            }
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _customerService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Customer was not found."
                }
            });
        }

        return Ok(new ApiResponse<object?>
        {
            Data = null,
            Messages = new List<string>
            {
                "Customer deleted successfully."
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
