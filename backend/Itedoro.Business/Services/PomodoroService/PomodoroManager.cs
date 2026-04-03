using Itedoro.Data;
using Itedoro.Data.Shared;
using Microsoft.EntityFrameworkCore;
using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.Utils;
using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Data.Repositories.Pomodoro.Interfaces;
using Itedoro.Business.Services.PomodoroService.Dtos;
using Itedoro.Business.Services.PomodoroService.Dtos.Requests;
using Itedoro.Business.Services.PomodoroService.Mappers;
using Itedoro.Business.Services.PomodoroService.Interfaces;
using Itedoro.Business.Services.PomodoroService.Dtos.Responses;

namespace Itedoro.Business.Services.PomodoroService;

public class PomodoroManager(
    ItedoroDbContext dbContext,
    PomodoroPlanGenerator planGenerator,
    IPomodoroRepository repository
) : IPomodoroService
{
    public async Task<Result<CreatePomodoroResponse>> CreateSessionAsync(Guid userId, PomodoroPreferencesDto dto)
    {
        
        var activeSession = await repository.FindActiveSessionAsync(userId); 
        
        if(activeSession != null)
        {
            activeSession.Status = PomodoroStatus.Canceled;
            activeSession.EndTime = DateTime.UtcNow;
        }

        var newSession = new ParentSession(userId, dto.TotalMinutes, dto.Note);
        var childPlans = planGenerator.Generate(dto);
        
        foreach (var plan in childPlans)
        {
            newSession.ChildSessions.Add(new ChildSession(newSession.Id, plan.Duration, plan.Type, plan.Order));
        }
        
        await dbContext.ParentSessions.AddAsync(newSession);
        var affectedRow = await dbContext.SaveChangesAsync();
        if (affectedRow == 0)
        {
            return Result<CreatePomodoroResponse>.Failure("No rows were affected.");
        }
        
        return Result<CreatePomodoroResponse>.Success(newSession.ToCreateResponseDto());
    }

    public async Task<Result<PausePomodororoResponse>> PauseSessionAsync(Guid userId, Guid parentId)
    {
        var activeSession = await repository.FindActiveSessionAsync(userId);
        if (activeSession == null || activeSession.Id != parentId)
        {
            return Result<PausePomodororoResponse>.Failure("There is no active session running.");
        }

        activeSession.Status = PomodoroStatus.Paused;
        activeSession.PauseStart = DateTime.UtcNow;
        foreach (var child in activeSession.ChildSessions)
        {
            if (child.Status != PomodoroStatus.Complated) 
            {
                child.Status = PomodoroStatus.Paused;
            }
        }

        await dbContext.SaveChangesAsync();
        return Result<PausePomodororoResponse>.Success(new PausePomodororoResponse(
            activeSession.Id,
            activeSession.Status.ToString(),
            DateTime.UtcNow));
    }

    public async Task<Result<ResumePomodoroResponse>> ResumeSessionAsync(Guid userId, Guid parentId)
    {
        var pausedSession = await repository.FindPausedSessionAsync(userId);
        if (pausedSession == null || pausedSession.Id != parentId)
        {
            return Result<ResumePomodoroResponse>.Failure("There is no paused session running.");
        }

        pausedSession.Status = PomodoroStatus.Running;
        pausedSession.PauseStop = DateTime.UtcNow;
        TimeSpan? diff = pausedSession.PauseStop - pausedSession.PauseStart;
        double diffMinutes = diff?.TotalMinutes ?? 0;
        pausedSession.EndTime = pausedSession.EndTime.AddMinutes(diffMinutes);
        
        foreach (var child in pausedSession.ChildSessions)
        {
            if (child.Status == PomodoroStatus.Paused)
            {
                child.Status = PomodoroStatus.Running;
            }
        }

        await dbContext.SaveChangesAsync();
        //TODO: Mapplenebilir
        return Result<ResumePomodoroResponse>.Success( new ResumePomodoroResponse(
            pausedSession.Id,
            pausedSession.Status.ToString(),
            pausedSession.TotalPlannedMinutes,
            pausedSession.EndTime));
    }

    public async Task<Result<StopPomodoroResponse>> StopSessionAsync(Guid userId, Guid parentId)
    {
        //Repositorye taşı
        var parent = await dbContext.ParentSessions
            .Include(c => c.ChildSessions)
            .FirstOrDefaultAsync(s => s.Id == parentId && s.UserId == userId);
        if (parent == null)
        {
            return Result<StopPomodoroResponse>.Failure("Session not found.");
        }

        if (parent.Status != PomodoroStatus.Running && parent.Status != PomodoroStatus.Paused)
        {
            return Result<StopPomodoroResponse>.Failure("This session is not active and cannot be stopped.");
        }

        parent.Status = PomodoroStatus.Canceled;
        parent.EndTime = DateTime.UtcNow;
        
        foreach (var child in parent.ChildSessions)
        {
            if (child.Status != PomodoroStatus.Complated)
            {
                child.Status = PomodoroStatus.Canceled;
            }
        }

        await dbContext.SaveChangesAsync();
        return Result<StopPomodoroResponse>.Success( new StopPomodoroResponse(
            parent.Id,
            parent.Status.ToString(),
            parent.EndTime));
    }

    public async Task<Result<PagedResult<GetPomodoroHistoryResponse>>> GetPagedSessionsAsync(Guid userId, GetPomodoroHistoryRequest dto)
    {
        var rawSessions = await repository.GetPagedParentsAsync(userId, dto.Page, dto.PageSize );
        var sessionDtos = rawSessions.Items.Select(s => s.CreateGetPomodoroHistoryResponseDto()).ToList();
        
        var pagedResponse = new PagedResult<GetPomodoroHistoryResponse>(
            sessionDtos,
            rawSessions.TotalCount,
            rawSessions.CurrentPage,
            rawSessions.PageSize);
        return Result<PagedResult<GetPomodoroHistoryResponse>>.Success(pagedResponse);
    }

    public async Task<Result<SkipBreakResponse>> SkipBreakAsync(Guid parentId, Guid childId)
    {
        var parent = await repository.FindParentSessionByParentIdAsync(parentId);
        if (parent == null)
        {
            return Result<SkipBreakResponse>.Failure("Parent session not found.");
        }

        var child = await repository.FindBreakByParentIdAndChildIdAsync(parentId, childId);
        if (child == null)
        {
            return Result<SkipBreakResponse>.Failure("Break session not found.");
        }
        
        
        child.Status = PomodoroStatus.Complated;
        parent.EndTime = parent.EndTime.AddMinutes(-child.PlannedDurationMinutes);
        await dbContext.SaveChangesAsync();
        return Result<SkipBreakResponse>.Success( new SkipBreakResponse(
            parent.Id,childId, child.Order+1,parent.EndTime));
    }

    public async Task<Result> DeleteSessionAsync(Guid parentId)
    {
        var result = await repository.DeleteSessionAsync(parentId);

        if (!result)
        {
            return Result.Failure("No rows were affected.");
        }

        return Result.Success();
    }
}