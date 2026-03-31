namespace Itedoro.Business.Services.PomodoroService.Dtos.Responses;

public record ResumePomodoroResponse(
    Guid ParentId,
    string Status,
    int TotalWorkMinutes,
    DateTime EndedAt
    );