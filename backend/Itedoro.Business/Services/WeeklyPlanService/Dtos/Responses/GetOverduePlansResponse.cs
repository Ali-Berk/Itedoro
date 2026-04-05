namespace Itedoro.Business.Services.WeeklyPlanService.Dtos.Responses;

public record GetOverduePlansResponse(
    Guid Id,
    string Title,
    string? Note,
    DateTime StartDate,
    DateTime EndDate,
    string ColorCode);