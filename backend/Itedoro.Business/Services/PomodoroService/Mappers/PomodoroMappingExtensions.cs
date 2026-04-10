using Itedoro.Business.Services.PomodoroService.Dtos.Responses;
using Itedoro.Data.Entities.PomodoroSessions.Enums;
using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Data.Shared;

namespace Itedoro.Business.Services.PomodoroService.Mappers;

public static class PomodoroMappingExtensions
{
    public static CreatePomodoroResponse CreateResponseMapper(this ParentSession session)
    {
        return new CreatePomodoroResponse(
            ParentId: session.Id,
            Status: session.Status.ToString(),
            StartedAt: session.StartTime,
            Childs: session.ChildSessions.Select(c => new PomodoroChildResponseDto(
                Id: c.Id,
                Type: c.Type.ToString(),
                DurationMinutes: c.PlannedDurationMinutes,
                Order: c.Order,
                Status: c.Status.ToString()
            )).ToList()
        );
    }

    public static GetPomodoroHistoryResponse GetPomodoroHistoryResponseMapper(this ParentSession session)
    {
        return new GetPomodoroHistoryResponse(
        Id: session.Id,
        Note: session.Note,
        Status: session.Status.ToString(),
        StartedAt: session.StartTime,
        EndedAt: session.EndTime,
        PlannedMinutes: session.TotalPlannedMinutes,
        CompletedChildCount: session.ChildSessions.Count(c => c.Status == PomodoroStatus.Complated),
        TotalChildCount: session.ChildSessions.Count
        );
    }

    public static ResumePomodoroResponse ResumePomodoroResponseMapper(this ParentSession session)
    {
        return new ResumePomodoroResponse(
            ParentId: session.Id,
            Status: session.Status.ToString(),
            TotalWorkMinutes: session.TotalPlannedMinutes,
            EndedAt: session.EndTime);
    }

    public static PausePomodoroResponse PausePomodoroResponseMapper(this ParentSession session)
    {
        var pauseStart = session.PauseStart;
        if (pauseStart == null)
        {
            throw new InvalidOperationException("PauseStart value cannot be null for a paused session..");
        }
        return new PausePomodoroResponse(
        ParentId: session.Id,
        NewStatus: session.Status.ToString(),
        UpdatedAt: pauseStart.Value);
    }

    public static StopPomodoroResponse StopPomodoroResponseMapper(this ParentSession session)
    {
        return new StopPomodoroResponse(
            ParentId: session.Id,
            Status: session.Status.ToString(),
            EndedAt: session.EndTime);
    }
}
