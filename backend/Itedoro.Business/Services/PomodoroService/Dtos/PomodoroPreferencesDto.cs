
namespace Itedoro.Business.Services.PomodoroService.Dtos;

public record PomodoroPreferencesDto(
    int TotalMinutes = 30,
    int WorkMinutes = 25,
    int ShortBreakMinutes = 5,
    int LongBreakMinutes = 30,
    int LongBreakInterval = 2
);