using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;

namespace Itedoro.Business.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is invalid.")
            .MaximumLength(150).WithMessage("Maximum lenght is 150 characters.");
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(15).WithMessage("Maximum lenght is 15 characters.");
        
        RuleFor(x => x.Name)
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("Name can only contain letters.")
            .MaximumLength(30).WithMessage("Maximum lenght is 30 characters.");
        
        RuleFor(x => x.Surname)
            .Matches(@"[a-zA-ZğüşıöçĞÜŞİÖÇ]+$").WithMessage("Surname can only contain letters.")
            .MaximumLength(30).WithMessage("Maximum lenght is 30 characters.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            
    }
}