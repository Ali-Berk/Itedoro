namespace Itedoro.Business.Services.WeeklyPlanService.Dtos;

public record CreatePlanRequest(
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string? Note,
    string? ColorCode
);