using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.WeeklyPlanService.Dtos;
using Itedoro.Data.Entities.WeeklyPlan;

namespace Itedoro.Business.Services.WeeklyPlanService;

public interface IWeeklyPlanService
{
    Task<Result<Guid>> CreatePlan(Guid userId, CreatePlanRequestDto request);
    Task<Result> UpdatePlan(Guid planItemId, UpdatePlanRequestDto request);
    Task<Result<List<PlanItem>>> GetAllPlans(Guid userId);
    Task<Result<List<PlanItem>>> GetSelectedPlans(Guid userId, DateTime startDate, DateTime endDate);
    Task<Result> DeletePlanItem(Guid userId, Guid planItemId);
}