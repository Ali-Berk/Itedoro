namespace Itedoro.Application.Services.PomodoroService.Interfaces;

public interface IPomodoroAuthorizationService
{
    Task<bool> IsOwnedByUserAsync(Guid userId, Guid parentId);
}