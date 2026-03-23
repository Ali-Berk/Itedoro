using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Business.Services.PomodoroService.Dtos;
using Itedoro.Data;
using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Business.Services.PomodoroService;

public class PomodoroManager(
    ItedoroDbContext dbContext,
    PomodoroPlanGenerator planGenerator
) : IPomodoroService
{
//DONE: Controller için validation ayarla.

    //Bu kısım geçici ileride repository olarak tekrar eklenilecek.
    private async Task<ParentSession?> FindActiveSessionAsync(Guid userId)
    {
        return await
            dbContext.ParentSessions.Include(p => p.ChildSessions).FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Running);
    }

    private async Task<List<ChildSession>> FindChildSessionsAsync(Guid parentId)
    {
        return await dbContext.ChildSessions.Where(c => c.ParentSessionId == parentId).OrderBy(o => o.Order).ToListAsync();
    }

    private async Task<ParentSession?> FindPausedSessionAsync(Guid userId)
    {
        return await
            dbContext.ParentSessions.FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Paused);
    }

    private async Task<List<ParentSession>> GetAllParentsAsync(Guid userId)
    {
        return await 
            dbContext.ParentSessions.Where(s => userId == s.UserId).ToListAsync();
    }

    private async Task<ParentSession?> FindParentSessionByParentIdAsync(Guid parentId)
    {
        return await 
            dbContext.ParentSessions.FirstOrDefaultAsync(p => p.Id == parentId);
    }
        
    public async Task<Result<ParentSession>> CreateSessionAsync(Guid userId, PomodoroPreferencesDto dto)
    {
        var activeSession = await FindActiveSessionAsync(userId); 
            
        if(activeSession != null)
        {
            activeSession.Status = PomodoroStatus.Canceled;
            activeSession.EndTime = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }
        var childPlans = planGenerator.Generate(dto);
        var newSession = new ParentSession(userId, dto.TotalMinutes, dto.Note);
        
        await dbContext.ParentSessions.AddAsync(newSession);
        foreach (var plan in childPlans)
        {
            var child = new ChildSession(newSession.Id, plan.Duration, plan.Type, plan.Order);
            dbContext.ChildSessions.Add(child);
        }
        await dbContext.SaveChangesAsync();
        return Result<ParentSession>.Success(newSession);
    }

    public async Task<Result> PauseSessionAsync(Guid userId, Guid parentId)
    {
        var activeSession = await FindActiveSessionAsync(userId);
        if (activeSession == null)
        {
            return Result.Failure("There is no pomodoro session running.");
        }
        activeSession.Status = PomodoroStatus.Paused;
        activeSession.PauseStart = DateTime.UtcNow;
        var childSessions = await FindChildSessionsAsync(parentId);
        foreach (var child in childSessions)
        {
            if (child.Status != PomodoroStatus.Complated) 
            {
                child.Status = PomodoroStatus.Paused;
            }
        }

        await dbContext.SaveChangesAsync();
        return Result.Success();

    }

    public async Task<Result> ResumeSessionAsync(Guid userId, Guid parentId)
    {
        var pausedSession = await FindPausedSessionAsync(userId);
        if (pausedSession == null)
        {
            return Result.Failure("There is no paused session running.");
        }

        pausedSession.Status = PomodoroStatus.Running;
        pausedSession.PauseStop = DateTime.UtcNow;
        TimeSpan? diff = pausedSession.PauseStop - pausedSession.PauseStart;
        double diffMinutes = diff?.TotalMinutes ?? 0;
        pausedSession.EndTime = pausedSession.EndTime.AddMinutes(diffMinutes);
        
        var childs = await FindChildSessionsAsync(parentId);
        foreach (var child in childs)
        {
            if (child.Status == PomodoroStatus.Paused)
            {
                child.Status = PomodoroStatus.Running;
            }
        }

        await dbContext.SaveChangesAsync();
        return Result.Success();
        

    }

    public async Task<Result> StopSessionAsync(Guid userId, Guid parentId)
    {
        var parent = await dbContext.ParentSessions.FirstOrDefaultAsync(s => s.Id == parentId && s.UserId == userId);
        if (parent == null)
        {
            return Result.Failure("Session not found.");
        }

        if (parent.Status != PomodoroStatus.Running && parent.Status != PomodoroStatus.Paused)
        {
            return Result.Failure("This session is not active and cannot be stopped.");
        }

        parent.Status = PomodoroStatus.Canceled;
        parent.EndTime = DateTime.UtcNow;
        
        var childs = await FindChildSessionsAsync(parentId);
        foreach (var child in childs)
        {
            if (child.Status != PomodoroStatus.Complated)
            {
                child.Status = PomodoroStatus.Canceled;
            }
        }

        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<List<ParentSession>>> GetAllSessionsAsync(Guid userId)
    {
        var sessions = await GetAllParentsAsync(userId);
        if (sessions.Count == 0)
        {
            return Result<List<ParentSession>>.Success(sessions);
        }
        foreach (var session in sessions)
        {
            await FindChildSessionsAsync(session.Id);
        }
        
        return Result<List<ParentSession>>.Success(sessions);
    }

    public async Task<Result> SkipBreakAsync(Guid parentId, Guid childId)
    {
        var parent = await FindParentSessionByParentIdAsync(parentId);
        if (parent == null)
        {
            return Result.Failure("Parent session not found.");
        }

        var child = await dbContext.ChildSessions.Where(c => c.Type != PomodoroType.Work).FirstOrDefaultAsync(c => c.Id == childId);
        if (child == null)
        {
            return Result.Failure("Break session not found.");
        }

        child.Status = PomodoroStatus.Complated;
        parent.EndTime = parent.EndTime.AddMinutes(-child.PlannedDurationMinutes);
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }
}