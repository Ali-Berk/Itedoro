using Itedoro.Data.Shared;
using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.Utils;
using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Data.Repositories.Pomodoro.Interfaces;
using Itedoro.Business.Services.PomodoroService.Dtos.Requests;
using Itedoro.Business.Services.PomodoroService.Mappers;
using Itedoro.Business.Services.PomodoroService.Interfaces;
using Itedoro.Business.Services.PomodoroService.Dtos.Responses;
using Itedoro.Data.Entities.PomodoroSessions.Enums;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Business.Services.PomodoroService;

public class PomodoroManager(
    PomodoroPlanGenerator planGenerator,
    IPomodoroRepository repository
) : IPomodoroService
{
    //Geliştirilebilir. Çocukları parent ekleyebilir ayrıca aktifleri domain üzerinden iptal edebiliriz.
    public async Task<Result<CreatePomodoroResponse>> CreateSessionAsync(Guid userId, CreatePomodoroRequest dto)
    {
        
        var activeSession = await repository.FindActiveSessionAsync(userId); 
        if(activeSession != null)
        {
            activeSession.Stop();
        }

        var newSession = new ParentSession(userId, dto.TotalMinutes, dto.Note);
        var childPlans = planGenerator.Generate(dto);
        
        foreach (var plan in childPlans)
        {
            newSession.ChildSessions.Add(new ChildSession(newSession.Id, plan.Duration, plan.Type, plan.Order));
        }
        
        await repository.AddAsync(newSession);
        await repository.SaveAsync();        
        return Result<CreatePomodoroResponse>.Success(newSession.CreateResponseMapper());
    }

    public async Task<Result<PausePomodoroResponse>> PauseSessionAsync(Guid userId, Guid parentId)
    {
        var activeSession = await repository.FindActiveSessionAsync(userId);
        if (activeSession == null || activeSession.Id != parentId)
            return Result<PausePomodoroResponse>.Failure("There is no matching active session.");
        
        activeSession.Pause();

        await repository.SaveAsync();
        return Result<PausePomodoroResponse>.Success(activeSession.PausePomodoroResponseMapper());
    }

    public async Task<Result<ResumePomodoroResponse>> ResumeSessionAsync(Guid userId, Guid parentId)
    {
        var pausedSession = await repository.FindPausedSessionAsync(userId);
        if (pausedSession == null || pausedSession.Id != parentId)
        {
            return Result<ResumePomodoroResponse>.Failure("There is no paused session running.");
        }
        pausedSession.Resume();
        await repository.SaveAsync();

        return Result<ResumePomodoroResponse>.Success(pausedSession.ResumePomodoroResponseMapper());
    }

    public async Task<Result<StopPomodoroResponse>> StopSessionAsync(Guid userId, Guid parentId)
    {
        var parent = await repository.GetUserSessionWithChildrenAsync(userId, parentId);
        if (parent == null)
            return Result<StopPomodoroResponse>.Failure("Session not found.");
        if (parent.Status != PomodoroStatus.Running && parent.Status != PomodoroStatus.Paused)
            return Result<StopPomodoroResponse>.Failure("This session is not active and cannot be stopped.");
        
        parent.Stop();
        await repository.SaveAsync();
        return Result<StopPomodoroResponse>.Success( parent.StopPomodoroResponseMapper());
    }

    //TODO: İki kere PagedResult değeri dönüyor bunun yerine repositoryde entity dön ve managerda paged resulta çevir.
    public async Task<Result<PagedResult<GetPomodoroHistoryResponse>>> GetPagedSessionsAsync(Guid userId, GetPomodoroHistoryRequest request)
    {
        var rawSessions = await repository.GetPagedParentsAsync(userId, request.Page, request.PageSize );
        var sessions = rawSessions.Items.Select(s => s.GetPomodoroHistoryResponseMapper()).ToList();
        
        var pagedResponse = new PagedResult<GetPomodoroHistoryResponse>(
            sessions,
            rawSessions.TotalCount,
            rawSessions.CurrentPage,
            rawSessions.PageSize);
        return Result<PagedResult<GetPomodoroHistoryResponse>>.Success(pagedResponse);
    }

    public async Task<Result<SkipBreakResponse>> SkipBreakAsync(Guid parentId, Guid childId)
    {
        var parent = await repository.FindSkippableBreakAsync(parentId, childId);
        if (parent == null)
        {
            return Result<SkipBreakResponse>.Failure("Break session not found.");
        }
        
        parent.SkipBreak(childId);
        await repository.SaveAsync();
        return Result<SkipBreakResponse>.Success( new SkipBreakResponse(
            parent.Id,childId, null,parent.EndTime));
    }

    // remove(session)
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