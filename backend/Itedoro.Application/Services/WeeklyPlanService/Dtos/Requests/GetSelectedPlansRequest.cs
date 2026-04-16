namespace Itedoro.Application.Services.WeeklyPlanService.Dtos.Requests;

public record GetSelectedPlansRequest(
    DateTime? StartDate,
    DateTime? EndDate,
    int Page = 1,
    int PageSize = 25);