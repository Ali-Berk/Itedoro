using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Data.Shared;

namespace Itedoro.Data.Repositories.Pomodoro.Interfaces;

public interface IPomodoroRepository
{
    Task<ParentSession?> FindActiveSessionAsync(Guid userId);
    Task<ParentSession?> FindPausedSessionAsync(Guid userId);
    Task<PagedResult<ParentSession>> GetPagedParentsAsync(Guid userId, int page, int record);
    Task<ParentSession?> FindParentSessionByParentIdAsync(Guid parentId);
    Task<ChildSession?> FindBreakByParentIdAndChildIdAsync(Guid parentId, Guid childId);
    Task<bool> DeleteSessionAsync(Guid parentId);
    Task<bool> IsUserTrue(Guid userId, Guid parentId);
}