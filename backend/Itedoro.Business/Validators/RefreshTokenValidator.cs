using FluentValidation;
using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Data.Entities.Users;

namespace Itedoro.Business.Validators;

public class RefreshTokenValidator : AbstractValidator<RefreshToken>
{
    public RefreshTokenValidator()
    {
        RuleFor(t => t.ExpiryTime)
            .GreaterThan(t => t.CreatedAt)
            .WithMessage("Token has expired.");

    }
}