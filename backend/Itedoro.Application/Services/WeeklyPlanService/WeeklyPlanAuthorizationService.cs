using Itedoro.Application.Services.WeeklyPlanService.Interfaces;
using Itedoro.Application.Repositories;

namespace Itedoro.Application.Services.WeeklyPlanService;

public class WeeklyPlanAuthorizationService(IWeeklyPlanRepository repository) : IWeeklyPlanAuthorizationService
{
    public async Task<bool> IsAuthorized(Guid userId, Guid planId)
    {
        return await repository.IsAuthorized(userId, planId);
    }
}
