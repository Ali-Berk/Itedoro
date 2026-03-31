using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Business.Services.PomodoroService.Dtos.Responses;

namespace Itedoro.Business.Services.PomodoroService.Interfaces;

public interface IPomodoroRepository
{
    Task<ParentSession?> FindActiveSessionAsync(Guid userId);
    Task<ParentSession?> FindPausedSessionAsync(Guid userId);
    Task<List<GetPomodoroHistoryResponse>> GetAllParentsAsync(Guid userId);
    Task<ParentSession?> FindParentSessionByParentIdAsync(Guid parentId);
    Task<ChildSession?> FindBreakByParentIdAndChildIdAsync(Guid parentId, Guid childId);
    Task<bool> DeleteSessionAsync(Guid parentId);
    Task<bool> IsUserTrue(Guid userId, Guid parentId);
}