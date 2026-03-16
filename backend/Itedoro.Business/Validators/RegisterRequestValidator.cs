using FluentValidation;
using Itedoro.Business.Services.RegisterService.Dtos;
using Itedoro.Business.Services.UserServices;

namespace Itedoro.Business.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email or Username is required.")
            .When(x => string.IsNullOrEmpty(x.Username))
            .EmailAddress()
            .WithMessage("Invalid email address.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username or Email is required.")
            .When(x => string.IsNullOrEmpty(x.Email));
    }
}