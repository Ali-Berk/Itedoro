using Itedoro.Application.Common.Errors;
using Itedoro.Application.Common.Shared.Results;
using Itedoro.Application.Services.WeeklyPlanService.Dtos.Requests;
using Itedoro.Application.Services.WeeklyPlanService.Dtos.Responses;
using Itedoro.Domain.Entities.WeeklyPlans;
using Itedoro.Application.Repositories;
using Itedoro.Application.Services.WeeklyPlanService.Mappers;
using Itedoro.Application.Services.WeeklyPlanService.Interfaces;

namespace Itedoro.Application.Services.WeeklyPlanService;

public class WeeklyPlanManager(
    IWeeklyPlanRepository repository): IWeeklyPlanService
{
    public async Task<Result<Guid>> CreatePlan(Guid userId, CreatePlanRequest newPlan)
    {
        var newPlanItem = new PlanItem(
            userId,
            newPlan.Title,
            newPlan.StartDate,
            newPlan.EndDate,
            newPlan.Note,
            newPlan.ColorCode);
        await repository.AddAsync(newPlanItem);
        await repository.SaveAsync();
        return Result<Guid>.Success(newPlanItem.Id);
    }
    public async Task<Result> UpdatePlan(Guid userId, UpdatePlanRequest request)
    {
        var planItem = await repository.GetByIdAsync(request.Id, userId);
        if (planItem == null) return CommonErrors.NotFound;
        
        planItem.UpdatePlan(
            request.Title,
            request.StartDate,
            request.EndDate,
            request.Note,
            request.ColorCode);
        await repository.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateStatus(Guid userId, Guid planId)
    {
        
        var planItem = await repository.GetByIdAsync(planId, userId);
        if (planItem == null) return CommonErrors.NotFound;
        
        planItem.UpdateStatus();
        await repository.SaveAsync();
        return Result.Success();
    }
    public async Task<Result> DeletePlanItem(Guid userId, Guid planItemId)
    {
        var selectedPlan = await repository.GetByIdAsync(planItemId, userId);
        
        if (selectedPlan == null) return CommonErrors.NotFound;

        repository.Delete(selectedPlan);
        await repository.SaveAsync();
        return Result.Success();
    }
    public async Task<Result<DatePagedResult<GetAllPlansPagedBetweenDatesResponse>>> GetAllPlansPagedBetweenDates(Guid userId, GetSelectedPlansRequest request)
    {
        var startDate = request.StartDate ?? DateTime.UtcNow;
        var endDate = request.EndDate ?? startDate.AddDays(7);

        var (rawSelectedPlans,hasMoreData) = await repository.GetPlansBetweenDatesAsync(userId, startDate, endDate, request.Page, request.PageSize);

        var selectedPlans =
            rawSelectedPlans
            .Select(planItem => planItem.GetAllPlansPagedBetweenDatesResponseMapper())
            .ToList();
        
        var result = new DatePagedResult<GetAllPlansPagedBetweenDatesResponse>(
            Items:selectedPlans,
            CurrentStartDate:startDate,
            CurrentEndDate:endDate,
            NextStartDate:hasMoreData ? endDate : null);
        return Result<DatePagedResult<GetAllPlansPagedBetweenDatesResponse>>.Success(result);
    }

    public async Task<List<GetOverduePlansResponse>> GetAllOverduePlans(Guid userId, DateTime referenceDate, CancellationToken cancellationToken)
    {
        if (referenceDate == default)
        {
            referenceDate = DateTime.UtcNow;
        }
        var rawOverduePlans = await repository.GetOverduePlansAsync(userId, referenceDate, cancellationToken);
        return rawOverduePlans.GetOverduePlansResponseMapper().ToList();
    }

    public async Task<List<GetUpcomingPlansResponse>> GetAllUpcomingPlans(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        if (startDate == default)
            startDate = DateTime.UtcNow;

        if (endDate == default)
            endDate = startDate.AddDays(7);

        var rawUpcomingPlans =
            await repository.GetUpcomingPlansAsync(userId, startDate, endDate, cancellationToken);
        return rawUpcomingPlans.GetUpcomingPlansResponseMapper().ToList();
    }
}
