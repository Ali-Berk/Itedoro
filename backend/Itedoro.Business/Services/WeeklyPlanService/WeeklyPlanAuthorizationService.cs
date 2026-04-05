using Itedoro.Business.Services.WeeklyPlanService.Interfaces;
using Itedoro.Data.Repositories.WeeklyPlan.Interfaces;

namespace Itedoro.Business.Services.WeeklyPlanService;

public class WeeklyPlanAuthorizationService(IWeeklyPlanRepository repository) : IWeeklyPlanAuthorizationService
{
    public async Task<bool> IsAuthorized(Guid userId, Guid planId)
    {
        return await repository.IsAuthorized(userId, planId);
    }
}