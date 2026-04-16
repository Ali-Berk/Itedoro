namespace Itedoro.Application.Services.WeeklyPlanService.Dtos.Responses;

public record GetAllPlansPagedBetweenDatesResponse(
    Guid Id,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string ColorCode,
    string? Note,
    bool IsCompleted);