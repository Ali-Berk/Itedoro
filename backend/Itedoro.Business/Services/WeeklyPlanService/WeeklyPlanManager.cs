using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.WeeklyPlanService.Dtos;
using Itedoro.Data;
using Itedoro.Data.Entities.WeeklyPlan;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Business.Services.WeeklyPlanService;


public class WeeklyPlanManager(
    ItedoroDbContext dbContext): IWeeklyPlanService
{
    public async Task<Result<Guid>> CreatePlan(Guid userId, CreatePlanRequestDto newPlan)
    {
        var newPlanItem = new PlanItem(
            userId,
            newPlan.Title,
            newPlan.StartDate,
            newPlan.EndDate,
            newPlan.Note,
            newPlan.ColorCode);

        dbContext.PlanItems.Add(newPlanItem);
        await dbContext.SaveChangesAsync();
        return Result<Guid>.Success(newPlanItem.Id);
    }

    public async Task<Result> UpdatePlan(Guid planItemId, UpdatePlanRequestDto request)
    {
        var planItem = await dbContext.PlanItems.FirstOrDefaultAsync(i => i.Id == planItemId);
        if (planItem == null)
        {
            return Result.Failure("Plan item not found.");
        }
        planItem.UpdatePlan(
            request.Title,
            request.StartDate,
            request.EndDate,
            request.Note,
            request.ColorCode);
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<List<PlanItem>>> GetAllPlans(Guid userId)
    {
        var allPlans = await dbContext.PlanItems
            .AsNoTracking()
            .Where(i => i.UserId == userId)
            .ToListAsync();
        if (!allPlans.Any())
        {
            return Result<List<PlanItem>>.Failure("No plans found");
        }
        return Result<List<PlanItem>>.Success(allPlans);
    }

    public async Task<Result<List<PlanItem>>> GetSelectedPlans(Guid userId, DateTime startDate, DateTime endDate)
    {
        var selectedPlans = await dbContext.PlanItems
            .AsNoTracking()
            .Where(i => i.UserId == userId)
            .Where(d => d.StartDate < endDate && d.EndDate > startDate)
            .ToListAsync();
        if (!selectedPlans.Any())
        {
            return Result<List<PlanItem>>.Failure("No plans found");
        }
        return Result<List<PlanItem>>.Success(selectedPlans);
    }

    public async Task<Result> DeletePlanItem(Guid userId, Guid planItemId)
    {
        var selectedPlan = await dbContext.PlanItems
            .Where(i => i.Id == planItemId && i.UserId == userId)
            .FirstOrDefaultAsync();
        if (selectedPlan == null)
        {
            return Result.Failure("No plans found");
        }

        dbContext.PlanItems.Remove(selectedPlan);
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }
}