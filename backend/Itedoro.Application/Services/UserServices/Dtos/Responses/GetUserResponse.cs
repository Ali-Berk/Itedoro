namespace Itedoro.Application.Services.UserServices.Dtos.Responses;

public record GetUserResponse(
    Guid UserId,
    string Username,
    string? Name,
    string? Surname,
    DateTime CreatedAt,
    
    int TotalCompletedPomodoros,
    int TotalCompletedWeeklyPlans,
    int TotalStudyTimeInMinutes);