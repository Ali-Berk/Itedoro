namespace Itedoro.Business.Services.WeeklyPlanService.Dtos.Requests;

public record CreatePlanRequest(
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string? Note,
    string? ColorCode
);