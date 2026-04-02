using Itedoro.Business.Services.Utils;
using Itedoro.Business.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;
using Itedoro.Business.Services.PomodoroService;
using Itedoro.Business.Daemons.TokenCleanupDaemon;
using Itedoro.Business.Services.WeeklyPlanService;
using Itedoro.Business.Services.AuthServices.LoginService;
using Itedoro.Business.Services.AuthServices.TokenService;
using Itedoro.Business.Services.PomodoroService.Interfaces;
using Itedoro.Business.Services.AuthServices.RegisterService;

namespace Itedoro.Business;

public static class BusinessDependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
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
        
        //Daemons
        services.AddHostedService<TokenCleanupDaemon>();
        
        //Utils / Helpers
        services.AddSingleton<PomodoroPlanGenerator>();
        
        return services;    
    }
}
