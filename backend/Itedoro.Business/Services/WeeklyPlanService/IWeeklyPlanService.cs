using Itedoro.Business.Shared.Result;
using Itedoro.Data.Entities.WeeklyPlan;
using Itedoro.Business.Services.WeeklyPlanService.Dtos.Requests;

namespace Itedoro.Business.Services.WeeklyPlanService;

public interface IWeeklyPlanService
{
    Task<Result<Guid>> CreatePlan(Guid userId, CreatePlanRequest request);
    Task<Result> UpdatePlan(Guid planItemId, UpdatePlanRequest request);
    Task<Result<List<PlanItem>>> GetAllPlans(Guid userId);
    Task<Result<List<PlanItem>>> GetSelectedPlans(Guid userId, DateTime startDate, DateTime endDate);
    Task<Result> DeletePlanItem(Guid userId, Guid planItemId);
}