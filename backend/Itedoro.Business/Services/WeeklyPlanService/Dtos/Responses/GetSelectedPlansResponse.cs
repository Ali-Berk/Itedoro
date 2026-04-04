namespace Itedoro.Business.Services.WeeklyPlanService.Dtos.Responses;

public record GetSelectedPlansResponse(
    Guid Id,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string? Note,
    string ColorCode,
    bool IsCompleted);