using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Data.Shared;
using Itedoro.Data.Repositories.Repository.Interfaces;

namespace Itedoro.Data.Repositories.Pomodoro.Interfaces;

public interface IPomodoroRepository : IRepository<ParentSession>
{
    Task<ParentSession?> FindActiveSessionAsync(Guid userId);
    Task<ParentSession?> FindPausedSessionAsync(Guid userId);
    Task<PagedResult<ParentSession>> GetPagedParentsAsync(Guid userId, int page, int record);
    Task<ParentSession?> GetUserSessionWithChildrenAsync(Guid userId, Guid parentId);
    Task<ParentSession?> FindSkippableBreakAsync(Guid parentId, Guid childId);
    Task<bool> DeleteSessionAsync(Guid parentId);
    Task<bool> IsUserTrue(Guid userId, Guid parentId);
}