using FluentValidation;
using Itedoro.Business.Services.LoginService.Dtos;

namespace Itedoro.Business.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.LoginHandle)
            .NotEmpty().WithMessage("Email or username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}