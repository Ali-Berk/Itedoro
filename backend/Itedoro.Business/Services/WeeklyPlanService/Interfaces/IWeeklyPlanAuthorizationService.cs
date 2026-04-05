using Itedoro.Data.Repositories.WeeklyPlan;

namespace Itedoro.Business.Services.WeeklyPlanService.Interfaces;

public interface IWeeklyPlanAuthorizationService
{
    Task<bool> IsAuthorized(Guid userId, Guid planId);
}