namespace Itedoro.Application.Services.PomodoroService.Dtos.Requests;

public record GetPomodoroHistoryRequest(
    int Page = 1,
    int PageSize = 25);