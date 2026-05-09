using Itedoro.Application.Services.PomodoroService.Interfaces;
using Itedoro.Application.Repositories;
namespace Itedoro.Application.Services.PomodoroService;

public class PomodoroAuthorizationService(
    IPomodoroRepository repository) : IPomodoroAuthorizationService
{
    public async Task<bool> IsOwnedByUserAsync(Guid userId, Guid parentId)
    {
        return await repository.IsOwnedByUserAsync(userId, parentId);
        
    }
}