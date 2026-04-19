namespace Itedoro.Application.Services.UserServices.Dtos.Responses;

public record GetMeResponse(
    Guid Id,
    string Username,
    string? Name,
    string? Surname,
    string Email,
    double PomodoroCompletionRate,
    int WeeklyCompletedPomodoros,
    int WeeklyPlanCount,
    double WeeklyPlanCompletionRate,
    int WeeklyStudyInMinutes,
    int TotalCompletedPomodoros,
    int TotalStudyTimeInMinutes
    );