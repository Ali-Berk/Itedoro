using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.WeeklyPlanService.Dtos.Requests;
using Itedoro.Business.Services.WeeklyPlanService.Dtos.Responses;
using Itedoro.Data.Shared;

namespace Itedoro.Business.Services.WeeklyPlanService.Interfaces;

public interface IWeeklyPlanService
{
    Task<Result<Guid>> CreatePlan(Guid userId, CreatePlanRequest request);
    Task<Result> UpdatePlan(Guid planItemId, UpdatePlanRequest request);
    Task<Result> UpdateStatus(Guid userId, Guid planId);
    Task<Result<DatePagedResult<GetAllPlansPagedBetweenDatesResponse>>> GetAllPlansPagedBetweenDates(Guid userId, GetSelectedPlansRequest request);
    Task<Result> DeletePlanItem(Guid userId, Guid planItemId);
    Task<List<GetOverduePlansResponse>> GetAllOverduePlans(Guid userId, DateTime referenceDate, CancellationToken cancellationToken);
    Task<List<GetUpcomingPlansResponse>> GetAllUpcomingPlans(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

}