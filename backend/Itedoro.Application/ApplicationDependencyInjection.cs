using Itedoro.Application.Services.Utils;
using Itedoro.Application.Services.UserServices.Interfaces;
using Itedoro.Application.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;
using Itedoro.Application.Services.PomodoroService;
using Itedoro.Application.Daemons;
using Itedoro.Application.Services.WeeklyPlanService;
using Itedoro.Application.Services.WeeklyPlanService.Interfaces;
using Itedoro.Application.Services.AuthServices.LoginService;
using Itedoro.Application.Services.AuthServices.LoginService.Interfaces;
using Itedoro.Application.Services.AuthServices.TokenService;
using Itedoro.Application.Services.AuthServices.TokenService.Interfaces;
using Itedoro.Application.Services.PomodoroService.Interfaces;
using Itedoro.Application.Services.AuthServices.RegisterService;
using Itedoro.Application.Services.AuthServices.RegisterService.Interfaces;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Itedoro.Application.Validators;
using Itedoro.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Itedoro.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //Services
        services.AddScoped<IRegisterService, RegisterManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<ILoginService, LoginManager>();
        services.AddScoped<ILoginStrategy, EmailLoginStrategy>();
        services.AddScoped<ILoginStrategy, UsernameLoginStrategy>();

        services.AddScoped<ITokenService, TokenManager>();
        
        services.AddScoped<IPomodoroService, PomodoroManager>();
        services.AddScoped<IPomodoroAuthorizationService, PomodoroAuthorizationService>();
        
        services.AddScoped<IWeeklyPlanService, WeeklyPlanManager>();
        services.AddScoped<IWeeklyPlanAuthorizationService, WeeklyPlanAuthorizationService>();
        
        //Daemons
        services.AddHostedService<TokenCleanupDaemon>();
        
        //Utils / Helpers
        services.AddSingleton<PomodoroPlanGenerator>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User> > ();
        return services;    
    }

    public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.DisableBuiltInModelValidation = true;
            configuration.EnableBodyBindingSourceAutomaticValidation = true;
        });
        return services;    
    }
}
