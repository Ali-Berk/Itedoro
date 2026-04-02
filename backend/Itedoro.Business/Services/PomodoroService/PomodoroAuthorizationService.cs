using Itedoro.Business.Services.PomodoroService.Interfaces;
using Itedoro.Data.Repositories.Pomodoro.Interfaces;
namespace Itedoro.Business.Services.PomodoroService;

public class PomodoroAuthorizationService(
    IPomodoroRepository repository) : IPomodoroAuthorizationService
{
    public Task<bool> IsUserTrue(Guid userId, Guid parentId)
    {
        return repository.IsUserTrue(userId, parentId);
        
    }
}