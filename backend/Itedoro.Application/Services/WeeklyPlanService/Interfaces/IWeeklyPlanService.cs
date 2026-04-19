using Itedoro.Application.Common.Shared.Results;
using Itedoro.Application.Services.WeeklyPlanService.Dtos.Requests;
using Itedoro.Application.Services.WeeklyPlanService.Dtos.Responses;

namespace Itedoro.Application.Services.WeeklyPlanService.Interfaces;

public interface IWeeklyPlanService
{
    Task<Result<Guid>> CreatePlan(Guid userId, CreatePlanRequest request);
    Task<Result> UpdatePlan(Guid userId, UpdatePlanRequest request);
    Task<Result> UpdateStatus(Guid userId, Guid planId);
    Task<Result<DatePagedResult<GetAllPlansPagedBetweenDatesResponse>>> GetAllPlansPagedBetweenDates(Guid userId, GetSelectedPlansRequest request);
    Task<Result> DeletePlanItem(Guid userId, Guid planItemId);
    Task<List<GetOverduePlansResponse>> GetAllOverduePlans(Guid userId, DateTime referenceDate, CancellationToken cancellationToken);
    Task<List<GetUpcomingPlansResponse>> GetAllUpcomingPlans(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

}
