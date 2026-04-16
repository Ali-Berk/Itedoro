namespace Itedoro.Application.Services.WeeklyPlanService.Dtos.Responses;

public record GetAllPlansResponse(
    Guid Id,
    string Title,
    string? Note,
    DateTime StartDate,
    DateTime EndDate,
    string ColorCode,
    bool IsCompleted);