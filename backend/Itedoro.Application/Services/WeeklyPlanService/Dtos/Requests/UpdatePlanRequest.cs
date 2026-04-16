namespace Itedoro.Application.Services.WeeklyPlanService.Dtos.Requests;

public record UpdatePlanRequest(
    Guid Id,
    string? Title,
    string? ColorCode,
    string? Note,
    DateTime? StartDate,
    DateTime? EndDate
);