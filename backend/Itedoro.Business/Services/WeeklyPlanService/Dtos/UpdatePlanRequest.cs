namespace Itedoro.Business.Services.WeeklyPlanService.Dtos;

public record UpdatePlanRequest(
    string? Title,
    string? ColorCode,
    string? Note,
    DateTime? StartDate,
    DateTime? EndDate
);