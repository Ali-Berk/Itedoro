using Itedoro.Data.Repositories.Pomodoro;
using Itedoro.Data.Repositories.Pomodoro.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Itedoro.Data.DependencyInjection;

public static class RepositoriesDependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection repositories)
    {
        repositories.AddScoped<IPomodoroRepository, PomodoroRepository>();
        
        return repositories;    
    }
}