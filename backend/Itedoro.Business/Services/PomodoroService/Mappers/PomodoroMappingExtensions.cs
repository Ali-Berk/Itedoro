using Itedoro.Business.Services.PomodoroService.Dtos.Responses;
using Itedoro.Data.Entities.PomodoroSessions;
namespace Itedoro.Business.Services.PomodoroService.Mappers;

public static class PomodoroMappingExtensions
{
    public static CreatePomodoroResponse ToCreateResponseDto(this ParentSession session)
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

    public static GetPomodoroHistoryResponse CreateGetPomodoroHistoryResponseDto(this ParentSession session)
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
}
