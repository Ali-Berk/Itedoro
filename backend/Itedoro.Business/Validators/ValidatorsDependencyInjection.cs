using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Itedoro.Business.Validators;

public static class ValidatorsDependencyInjection
{
    public static IServiceCollection AddBusinessValidators(this IServiceCollection validators)
    {
        validators.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        validators.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.DisableBuiltInModelValidation = true;
            configuration.EnableBodyBindingSourceAutomaticValidation = true;
        });
        return validators;    
    }
}