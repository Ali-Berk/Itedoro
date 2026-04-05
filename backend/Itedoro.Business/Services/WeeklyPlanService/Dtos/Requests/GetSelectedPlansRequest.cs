namespace Itedoro.Business.Services.WeeklyPlanService.Dtos.Requests;

public record GetSelectedPlansRequest(
    DateTime? StartDate,
    DateTime? EndDate);