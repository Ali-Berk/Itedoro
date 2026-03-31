namespace Itedoro.Business.Services.PomodoroService.Dtos.Responses;

public record PausePomodororoResponse(
    Guid ParentId,
    string NewStatus,
    DateTime UpdatedAt);