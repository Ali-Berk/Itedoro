using Itedoro.Application.Repositories;
using Itedoro.Persistence.Repositories;
using Itedoro.Persistence.Repositories.Auth;
using Itedoro.Persistence.Repositories.Pomodoro;
using Itedoro.Persistence.Repositories.RefreshToken;
using Itedoro.Persistence.Repositories.Repository;
using Itedoro.Persistence.Repositories.UserRepositories;
using Itedoro.Persistence.Repositories.UserStatRepositories;
using Itedoro.Persistence.Repositories.WeeklyPlan;
using Microsoft.Extensions.DependencyInjection;

namespace Itedoro.Persistence.DependencyInjection;

public static class RepositoriesDependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection repositories)
    {
        repositories.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        repositories.AddScoped<IPomodoroRepository, PomodoroRepository>();
        repositories.AddScoped<IWeeklyPlanRepository, WeeklyPlanRepository>();
        repositories.AddScoped<IAuthRepository, AuthRepository>();
        repositories.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        repositories.AddScoped<IUserRepository, UserRepository>();
        repositories.AddScoped<IRoleRepository, RoleRepository>();
        
        repositories.AddScoped<IUserWeekStatRepository, UserWeekStatRepository>();
        repositories.AddScoped<IUserTotalStatRepository, UserTotalStatRepository>();
        
        
        
        return repositories;    
    }
}
