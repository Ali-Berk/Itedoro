namespace Itedoro.Business.Services.WeeklyPlanService.Dtos.Requests;

public record UpdatePlanRequest(
    string? Title,
    string? ColorCode,
    string? Note,
    DateTime? StartDate,
    DateTime? EndDate
);