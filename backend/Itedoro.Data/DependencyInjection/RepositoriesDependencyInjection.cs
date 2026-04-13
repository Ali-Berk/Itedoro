using Itedoro.Data.Repositories.Auth;
using Itedoro.Data.Repositories.Auth.Interfaces;
using Itedoro.Data.Repositories.Pomodoro;
using Itedoro.Data.Repositories.Pomodoro.Interfaces;
using Itedoro.Data.Repositories.RefreshToken;
using Itedoro.Data.Repositories.RefreshToken.Interfaces;
using Itedoro.Data.Repositories.Repository.Interfaces;
using Itedoro.Data.Repositories.Repository;
using Itedoro.Data.Repositories.WeeklyPlan;
using Itedoro.Data.Repositories.WeeklyPlan.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Itedoro.Data.DependencyInjection;

public static class RepositoriesDependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection repositories)
    {
        repositories.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        repositories.AddScoped<IPomodoroRepository, PomodoroRepository>();
        repositories.AddScoped<IWeeklyPlanRepository, WeeklyPlanRepository>();
        repositories.AddScoped<IAuthRepository, AuthRepository>();
        repositories.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        return repositories;    
    }
}