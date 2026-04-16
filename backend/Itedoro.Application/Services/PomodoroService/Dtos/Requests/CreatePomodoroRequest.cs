namespace Itedoro.Application.Services.PomodoroService.Dtos.Requests;

public record CreatePomodoroRequest(
    int TotalMinutes = 30,
    int WorkMinutes = 25,
    int ShortBreakMinutes = 5,
    int LongBreakMinutes = 30,
    int LongBreakInterval = 2,
    string Note = ""
);