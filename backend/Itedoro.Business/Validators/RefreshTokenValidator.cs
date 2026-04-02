using FluentValidation;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;

namespace Itedoro.Business.Validators;

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