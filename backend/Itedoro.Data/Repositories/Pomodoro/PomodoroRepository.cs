using Itedoro.Data.Shared;
using Microsoft.EntityFrameworkCore;
using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Data.Repositories.Pomodoro.Interfaces;

namespace Itedoro.Data.Repositories.Pomodoro;

public class PomodoroRepository(
    ItedoroDbContext dbContext) : IPomodoroRepository
{
    public async Task<ParentSession?> FindActiveSessionAsync(Guid userId)
    {
        return await
            dbContext.ParentSessions
                .Include(p => p.ChildSessions)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Running);
    }
    public async Task<ParentSession?> FindPausedSessionAsync(Guid userId)
    {
        return await
            dbContext.ParentSessions
                .Include(c => c.ChildSessions)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Paused);
    }

    public async Task<PagedResult<ParentSession>> GetPagedParentsAsync(Guid userId, int page, int pageSize)
    {
        var totalCount = await dbContext.ParentSessions.CountAsync(s => s.UserId == userId);
        
        var sessions = await
            dbContext.ParentSessions
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .OrderByDescending(d => d.StartTime)
                .Skip((page - 1)*pageSize)
                .Take(pageSize)
                .Include(c => c.ChildSessions)
                .AsSplitQuery()
                .ToListAsync();
        return new PagedResult<ParentSession>(sessions, totalCount, page, pageSize);
    }

    public async Task<ParentSession?> FindParentSessionByParentIdAsync(Guid parentId)
    {
        return await 
            dbContext.ParentSessions
                .FirstOrDefaultAsync(p => p.Id == parentId);
    }

    public async Task<ChildSession?> FindBreakByParentIdAndChildIdAsync(Guid parentId, Guid childId)
    {
        return await dbContext.ChildSessions
            .Where(c => c.ParentSessionId == parentId && c.Type != PomodoroType.Work && c.Id == childId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteSessionAsync(Guid parentId)
    {
        var row = await dbContext.ParentSessions
            .Where(p => p.Id == parentId)
            .ExecuteDeleteAsync();
        
        return row > 0;
    }

    public async Task<bool> IsUserTrue(Guid userId, Guid parenId)
    {
        return await dbContext.ParentSessions.AnyAsync(p => p.UserId == userId && p.Id == parenId);
    }
}