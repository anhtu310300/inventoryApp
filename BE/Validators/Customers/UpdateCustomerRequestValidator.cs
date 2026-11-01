using FluentValidation;
using Inventory.Api.DTOs.Customers;

namespace Inventory.Api.Validators.Customers;

public class UpdateCustomerRequestValidator
    : AbstractValidator<UpdateCustomerRequestDto>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(request => request.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MaximumLength(150)
            .WithMessage(
                "Full name must not exceed 150 characters.");

        RuleFor(request => request.Email)
            .EmailAddress()
            .WithMessage("Email is invalid.")
            .MaximumLength(150)
            .WithMessage("Email must not exceed 150 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.Email));

        RuleFor(request => request.Phone)
            .MaximumLength(30)
            .WithMessage("Phone must not exceed 30 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.Phone));

        RuleFor(request => request.Gstin)
            .MaximumLength(30)
            .WithMessage("GSTIN must not exceed 30 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.Gstin));

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

        RuleFor(request => request.PostalCode)
            .MaximumLength(20)
            .WithMessage(
                "Postal code must not exceed 20 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.PostalCode));
    }
}
