namespace Itedoro.Business.Services.WeeklyPlanService.Dtos;

public record CreatePlanRequestDto(
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string? Note,
    string? ColorCode,
    Guid UserId 
);