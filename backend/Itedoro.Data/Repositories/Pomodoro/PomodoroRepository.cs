using Itedoro.Data.Shared;
using Microsoft.EntityFrameworkCore;
using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Data.Repositories.Pomodoro.Interfaces;
using Itedoro.Data.Repositories.Repository;

namespace Itedoro.Data.Repositories.Pomodoro;

public class PomodoroRepository(
    ItedoroDbContext context) : Repository<ParentSession>(context), IPomodoroRepository
{
    public async Task<ParentSession?> FindActiveSessionAsync(Guid userId)
    {
        return await
            Context.ParentSessions
                .Include(p => p.ChildSessions)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Running);
    }
    public async Task<ParentSession?> FindPausedSessionAsync(Guid userId)
    {
        return await
            Context.ParentSessions
                .Include(c => c.ChildSessions)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Paused);
    }

    public async Task<PagedResult<ParentSession>> GetPagedParentsAsync(Guid userId, int page, int pageSize)
    {
        var totalCount = await Context.ParentSessions.CountAsync(s => s.UserId == userId);
        
        var sessions = await
            Context.ParentSessions
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
    
    public async Task<ParentSession?> FindSkippableBreakAsync(Guid parentId, Guid childId)
    {
        return await Context.ParentSessions
            .Include(p => p.ChildSessions
                .Where(c => 
                c.Id == childId && 
                c.Type != PomodoroType.Work &&
                c.Status == PomodoroStatus.Running))
            .FirstOrDefaultAsync(p => p.Id == parentId);
    }

    public async Task<ParentSession?> GetUserSessionWithChildrenAsync(Guid userId, Guid parentId)
    {
        return await Context.ParentSessions
            .Include(c => c.ChildSessions)
            .FirstOrDefaultAsync(s => s.Id == parentId && s.UserId == userId);
    }
    public async Task<bool> DeleteSessionAsync(Guid parentId)
    {
        var row = await Context.ParentSessions
            .Where(p => p.Id == parentId)
            .ExecuteDeleteAsync();
        
        return row > 0;
    }

    public async Task<bool> IsUserTrue(Guid userId, Guid parenId)
    {
        return await Context.ParentSessions.AnyAsync(p => p.UserId == userId && p.Id == parenId);
    }
}