using FluentValidation;
using FluentValidation.Results;
using Inventory.Api.Common.Responses;
using Inventory.Api.DTOs.Categories;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IValidator<CreateCategoryRequestDto>
        _createValidator;
    private readonly IValidator<UpdateCategoryRequestDto>
        _updateValidator;

    public CategoriesController(
        ICategoryService categoryService,
        IValidator<CreateCategoryRequestDto> createValidator,
        IValidator<UpdateCategoryRequestDto> updateValidator)
    {
        _categoryService = categoryService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();

        return Ok(new ApiResponse<List<CategoryResponseDto>>
        {
            Data = categories,
            Messages = new List<string>
            {
                "Categories retrieved successfully."
            }
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Category was not found."
                }
            });
        }

        return Ok(new ApiResponse<CategoryResponseDto>
        {
            Data = category,
            Messages = new List<string>
            {
                "Category retrieved successfully."
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequestDto request)
    {
        var validationResult =
            await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var category = await _categoryService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            new ApiResponse<CategoryResponseDto>
            {
                Data = category,
                Messages = new List<string>
                {
                    "Category created successfully."
                }
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCategoryRequestDto request)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationFailure(validationResult);
        }

        var category =
            await _categoryService.UpdateAsync(id, request);

        if (category is null)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Category was not found."
                }
            });
        }

        return Ok(new ApiResponse<CategoryResponseDto>
        {
            Data = category,
            Messages = new List<string>
            {
                "Category updated successfully."
            }
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object?>
            {
                Data = null,
                Messages = new List<string>
                {
                    "Category was not found."
                }
            });
        }

        return Ok(new ApiResponse<object?>
        {
            Data = null,
            Messages = new List<string>
            {
                "Category deactivated successfully."
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
