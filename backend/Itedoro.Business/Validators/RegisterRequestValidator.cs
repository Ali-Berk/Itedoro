using FluentValidation;
using Itedoro.Business.Services.RegisterService.Dtos;
using Itedoro.Business.Services.UserServices;

namespace Itedoro.Business.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID is required.");
            
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.");
    }
}