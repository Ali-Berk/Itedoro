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
//DONE: Aktif olan 1 adet pomodoro olması gerekli. Kontrolünü sağla.
//TODO: Controller için validation ayarla.
//DONE: Pause ve Resume ekle.(Not: Bitiş süresini ilerletmen gerekecek.)
//TODO: Childların çalışma sırası için bir order ekle.

    private async Task<ParentSession?> FindActiveSessionAsync(Guid userId)
    {
        return await
            dbContext.ParentSessions.FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Running);
    }

    private async Task<List<ChildSession>> FindChildSessionsAsync(Guid parentId)
    {
        return await dbContext.ChildSessions.Where(c => c.ParentSessionId == parentId).ToListAsync();
    }

    private async Task<ParentSession?> FindPausedSessionAsync(Guid userId)
    {
        return await
            dbContext.ParentSessions.FirstOrDefaultAsync(s => s.UserId == userId && s.Status == PomodoroStatus.Paused);
    }
        
    public async Task<Result<ParentSession>> CreateSessionAsync(Guid userId, PomodoroPreferencesDto dto)
    {
        var activeSession = await FindActiveSessionAsync(userId); 
            
        if(activeSession != null)
        {
            activeSession.Status = PomodoroStatus.Paused;
            await dbContext.SaveChangesAsync();
        }
        var childPlans = planGenerator.Generate(dto);
        var newSession = new ParentSession(out Guid parentId, userId, dto.TotalMinutes);
        var parent = await dbContext.ParentSessions.AddAsync(newSession);
        foreach (var plan in childPlans)
        {
            var child = new ChildSession(parentId, plan.Duration, plan.Type);
            dbContext.ChildSessions.Add(child);
        }
        await dbContext.SaveChangesAsync();
        return Result<ParentSession>.Success(newSession);
    }

    //WARN: Complate olmayan childlar da pause edilmeli.
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
        var childs = await FindChildSessionsAsync(parentId);

        if (pausedSession == null)
        {
            return Result.Failure("There is no paused session running.");
        }

        pausedSession.Status = PomodoroStatus.Running;
        pausedSession.PauseStop = DateTime.UtcNow;
        TimeSpan? diff = pausedSession.PauseStop - pausedSession.PauseStart;
        double diffMinutes = diff.HasValue ? diff.Value.TotalMinutes : 0;
        pausedSession.EndTime = pausedSession.EndTime.AddMinutes(diffMinutes);
        
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
    
}