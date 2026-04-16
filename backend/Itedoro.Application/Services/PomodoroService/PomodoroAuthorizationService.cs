using Itedoro.Application.Services.PomodoroService.Interfaces;
using Itedoro.Application.Repositories;
namespace Itedoro.Application.Services.PomodoroService;

public class PomodoroAuthorizationService(
    IPomodoroRepository repository) : IPomodoroAuthorizationService
{
    public Task<bool> IsUserTrue(Guid userId, Guid parentId)
    {
        return repository.IsUserTrue(userId, parentId);
        
    }
}