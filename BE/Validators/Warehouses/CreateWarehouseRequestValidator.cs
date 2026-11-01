using FluentValidation;
using Inventory.Api.DTOs.Warehouses;

namespace Inventory.Api.Validators.Warehouses;

public class CreateWarehouseRequestValidator
    : AbstractValidator<CreateWarehouseRequestDto>
{
    public CreateWarehouseRequestValidator()
    {
        RuleFor(request => request.WarehouseName)
            .NotEmpty()
            .WithMessage("Warehouse name is required.")
            .MaximumLength(150)
            .WithMessage(
                "Warehouse name must not exceed 150 characters.");

        RuleFor(request => request.Address)
            .MaximumLength(255)
            .WithMessage("Address must not exceed 255 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.Address));

        RuleFor(request => request.City)
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.City));

        RuleFor(request => request.State)
            .MaximumLength(100)
            .WithMessage("State must not exceed 100 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.State));
    }
}
