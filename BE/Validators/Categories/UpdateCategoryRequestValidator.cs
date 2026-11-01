using FluentValidation;
using Inventory.Api.DTOs.Categories;

namespace Inventory.Api.Validators.Categories;

public class UpdateCategoryRequestValidator
    : AbstractValidator<UpdateCategoryRequestDto>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MaximumLength(100)
            .WithMessage(
                "Category name must not exceed 100 characters.");

        RuleFor(request => request.Description)
            .MaximumLength(255)
            .WithMessage(
                "Description must not exceed 255 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.Description));
    }
}
