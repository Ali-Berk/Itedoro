using FluentValidation;
using Itedoro.Application.Services.AuthServices.Dtos.Requests;

namespace Itedoro.Application.Validators;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(t => t.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token required.");
        RuleFor(a => a.AccessToken)
            .NotEmpty()
            .WithMessage("Access token required.");
    }
}