using FluentValidation;
using Inventory.Api.DTOs.Employees;

namespace Inventory.Api.Validators.Employees;

public class UpdateEmployeeRequestValidator
    : AbstractValidator<UpdateEmployeeRequestDto>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(request => request.UserName)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MaximumLength(30)
            .WithMessage(
                "Username must not exceed 30 characters.");

        RuleFor(request => request.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MaximumLength(150)
            .WithMessage(
                "Full name must not exceed 150 characters.");

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email is invalid.")
            .MaximumLength(150)
            .WithMessage(
                "Email must not exceed 150 characters.");

        RuleFor(request => request.Phone)
            .MaximumLength(30)
            .WithMessage(
                "Phone must not exceed 30 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.Phone));

        RuleFor(request => request.Role)
            .NotEmpty()
            .WithMessage("Role is required.")
            .MaximumLength(100)
            .WithMessage(
                "Role must not exceed 100 characters.");

        RuleFor(request => request.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .MaximumLength(30)
            .WithMessage(
                "Status must not exceed 30 characters.");

        RuleFor(request => request.Password)
            .MinimumLength(8)
            .WithMessage(
                "Password must contain at least 8 characters.")
            .MaximumLength(100)
            .WithMessage(
                "Password must not exceed 100 characters.")
            .When(request =>
                !string.IsNullOrWhiteSpace(request.Password));
    }
}
