using Microsoft.Extensions.DependencyInjection;
using Itedoro.Business.Services.RegisterService;
using Itedoro.Business.Services.UserServices;
using Itedoro.Business.Services.LoginService;
using Itedoro.Business.Services.TokenService;
using Itedoro.Business.Daemons.TokenCleanupDaemon;




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

        //Daemons
        services.AddHostedService<TokenCleanupDaemon>();
        return services;    
    }
}
