using Itedoro.Domain.Entities.PomodoroSessions;
using Itedoro.Application.Common.Models;
using Itedoro.Application.Repositories;

namespace Itedoro.Application.Repositories;

public interface IPomodoroRepository : IRepository<ParentSession>
{
    Task<ParentSession?> FindActiveSessionAsync(Guid userId);
    Task<ParentSession?> FindPausedSessionAsync(Guid userId);
    Task<PagedResult<ParentSession>> GetPagedParentsAsync(Guid userId, int page, int record);
    Task<ParentSession?> GetUserSessionWithChildrenAsync(Guid userId, Guid parentId);
    Task<ParentSession?> FindSkippableBreakAsync(Guid parentId, Guid childId);
    Task<bool> DeleteSessionAsync(Guid parentId);
    Task<bool> IsOwnedByUserAsync(Guid userId, Guid parentId);
}