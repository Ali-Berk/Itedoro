namespace Itedoro.Business.Services.PomodoroService.Dtos.Responses;

public record PausePomodoroResponse(
    Guid ParentId,
    string NewStatus,
    DateTime UpdatedAt);