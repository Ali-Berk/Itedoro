namespace Itedoro.Business.Services.WeeklyPlanService.Dtos;

public record UpdatePlanRequestDto(
    string? Title,
    string? ColorCode,
    string? Note,
    DateTime? StartDate,
    DateTime? EndDate
);