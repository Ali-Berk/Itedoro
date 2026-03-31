namespace Itedoro.Business.Services.PomodoroService.Interfaces;

public interface IPomodoroAuthorizationService
{
    Task<bool> IsUserTrue(Guid userId, Guid parentId);
}