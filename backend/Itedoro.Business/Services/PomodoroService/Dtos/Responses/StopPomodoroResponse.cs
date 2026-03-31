namespace Itedoro.Business.Services.PomodoroService.Dtos.Responses;

public record StopPomodoroResponse(
    Guid ParentId,
    string Status,
    DateTime EndedAt);
