using Microsoft.AspNetCore.Mvc;
using Itedoro.Business.Services.WeeklyPlanService;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Itedoro.Business.Services.WeeklyPlanService.Dtos;

namespace Itedoro.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WeeklyPlanController(
IWeeklyPlanService weeklyPlanManager
) : ControllerBase
{
    [HttpPost("createPlan")]
    public async Task<IActionResult> CreateNewPlan([FromBody] CreatePlanRequestDto createPlanRequest)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(userIdString, out Guid userId);
        var result = await weeklyPlanManager.CreatePlan(userId, createPlanRequest);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Value);
    }
    
    [HttpGet("selectedPlans/{startDate}/{endDate}")]
    public async Task<IActionResult> GetSelectedPlans(DateTime startDate, DateTime endDate)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(userIdString, out Guid userId);
        var result = await weeklyPlanManager.GetSelectedPlans(userId, startDate, endDate);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }
    
    [HttpGet("AllPlans")]
    public async Task<IActionResult> GetAllPlans()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(userIdString, out Guid userId);
        var result = await weeklyPlanManager.GetAllPlans(userId);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }

    [HttpPatch("UpdatePlan/{planItemId}")]
    public async Task<IActionResult> UpdatePlan([FromBody] UpdatePlanRequestDto updatePlanRequest, Guid planItemId)
    {
        // var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Guid.TryParse(userIdString, out Guid userId);
        //TODO: UserId de yollanacak.
        var result = await weeklyPlanManager.UpdatePlan(planItemId, updatePlanRequest);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }

    [HttpDelete("DeletePlanItem/{planItemId}")]
    public async Task<IActionResult> DeletePlanItem(Guid planItemId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(userIdString, out Guid userId);
        //TODO: UserId de yollanacak.
        var result = await weeklyPlanManager.DeletePlanItem(planItemId, userId);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }
}