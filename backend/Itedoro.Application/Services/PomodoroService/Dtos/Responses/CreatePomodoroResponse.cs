namespace Itedoro.Application.Services.PomodoroService.Dtos.Responses;

public record CreatePomodoroResponse(
    Guid ParentId,
    string Status,
    DateTime StartedAt,
    List<PomodoroChildResponseDto> Childs);
    
public record PomodoroChildResponseDto(
    Guid Id, 
    string Type, 
    int DurationMinutes, 
    int Order, 
    string Status);