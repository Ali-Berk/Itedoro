namespace Itedoro.Business.Services.PomodoroService.Dtos.Responses;

public record GetPomodoroHistoryResponse(
    Guid Id,
    string? Note,
    string Status,
    DateTime StartedAt,
    DateTime? EndedAt,
    int PlannedMinutes,
    int CompletedChildCount,
    int TotalChildCount);