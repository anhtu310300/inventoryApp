using FluentValidation;
using Inventory.Api.DTOs.Suppliers;

namespace Inventory.Api.Validators.Suppliers;

public class CreateSupplierRequestValidator
    : AbstractValidator<CreateSupplierRequestDto>
{
    public CreateSupplierRequestValidator()
    {
        RuleFor(request => request.SupplierName)
            .NotEmpty()
            .WithMessage("Supplier name is required.")
            .MaximumLength(150)
            .WithMessage(
                "Supplier name must not exceed 150 characters.");

        RuleFor(request => request.Gstin)
            .MaximumLength(30)
            .WithMessage("GSTIN must not exceed 30 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.Gstin));

        RuleFor(request => request.ContactName)
            .MaximumLength(150)
            .WithMessage(
                "Contact name must not exceed 150 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.ContactName));

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

        RuleFor(request => request.Address)
            .MaximumLength(255)
            .WithMessage("Address must not exceed 255 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.Address));

        RuleFor(request => request.PaymentTerms)
            .MaximumLength(50)
            .WithMessage(
                "Payment terms must not exceed 50 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.PaymentTerms));
    }
}
