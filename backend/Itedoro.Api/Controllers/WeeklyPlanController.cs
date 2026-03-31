using Itedoro.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Itedoro.Business.Services.WeeklyPlanService;
using Itedoro.Business.Services.WeeklyPlanService.Dtos;
namespace Itedoro.Api.Controllers;

[Authorize]
[ApiController]
[Route("weekly-plan")]

public class WeeklyPlanController(
IWeeklyPlanService weeklyPlanManager
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateNewPlan([FromBody] CreatePlanRequest createPlanRequest)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();
        var result = await weeklyPlanManager.CreatePlan(userId, createPlanRequest);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Value);
    }
    
    [HttpGet("selected-plans")]
    public async Task<IActionResult> GetSelectedPlans([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();
        var result = await weeklyPlanManager.GetSelectedPlans(userId, startDate, endDate);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPlans()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();
        var result = await weeklyPlanManager.GetAllPlans(userId);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("update-plan/{planItemId}")]
    public async Task<IActionResult> UpdatePlan([FromBody] UpdatePlanRequest updatePlanRequest, Guid planItemId)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();
        //TODO: UserId de yollanacak.
        var result = await weeklyPlanManager.UpdatePlan(planItemId, updatePlanRequest);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }

    [HttpDelete("delete-plan/{planItemId}")]
    public async Task<IActionResult> DeletePlanItem(Guid planItemId)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();
        var result = await weeklyPlanManager.DeletePlanItem(userId, planItemId);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }
}