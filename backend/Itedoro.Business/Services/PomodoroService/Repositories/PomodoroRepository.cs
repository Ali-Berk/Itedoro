using Itedoro.Data.Entities.PomodoroSessions;
using Microsoft.EntityFrameworkCore;
using Itedoro.Data;
using Itedoro.Business.Services.PomodoroService.Dtos.Responses;
using Itedoro.Business.Services.PomodoroService.Interfaces;
using Itedoro.Business.Services.PomodoroService.Mappers;

namespace Itedoro.Business.Services.PomodoroService.Repositories;

public class PomodoroRepository(
    ItedoroDbContext dbContext) : IPomodoroRepository
{
    public async Task<ParentSession?> FindActiveSessionAsync(Guid userId)
    {
        return await
            dbContext.ParentSessions.Include(p => p.ChildSessions).FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Running);
    }
    public async Task<ParentSession?> FindPausedSessionAsync(Guid userId)
    {
        return await
            dbContext.ParentSessions.Include(c => c.ChildSessions).FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Paused);
    }

    public async Task<List<GetPomodoroHistoryResponse>> GetAllParentsAsync(Guid userId)
    {
        return await 
            dbContext.ParentSessions
                .Where(s => userId == s.UserId)
                .Include(c => c.ChildSessions)
                .AsAsyncEnumerable()
                .Select(s => s.CreateGetPomodoroHistoryResponseDto())
                .ToListAsync();
    }

    public async Task<ParentSession?> FindParentSessionByParentIdAsync(Guid parentId)
    {
        return await 
            dbContext.ParentSessions.FirstOrDefaultAsync(p => p.Id == parentId);
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
        var session = await dbContext.ParentSessions.FindAsync(parenId);
        
        if (session != null && session.UserId == userId)
        {
            return true;
        }
        return false;
    }
}