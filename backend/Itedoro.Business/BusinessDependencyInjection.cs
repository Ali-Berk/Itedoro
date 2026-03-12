using Microsoft.Extensions.DependencyInjection;
using Itedoro.Business.Services.RegisterService;
using Itedoro.Business.Services.UserServices;
using Itedoro.Business.Services.LoginService;




namespace Itedoro.Business;

public static class BusinessDependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IRegisterService, RegisterManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<ILoginService, LoginManager>();
        services.AddScoped<ILoginStrategy, EmailLoginStrategy>();
        services.AddScoped<ILoginStrategy, UsernameLoginStrategy>();

        return services;    
    }
}