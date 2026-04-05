using Itedoro.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Itedoro.Data.Shared;
using Itedoro.Business.Services.WeeklyPlanService.Interfaces;
using Itedoro.Business.Services.WeeklyPlanService.Dtos.Requests;
using Itedoro.Business.Services.WeeklyPlanService.Dtos.Responses;

namespace Itedoro.Api.Controllers;

[Authorize]
[ApiController]
[Route("weekly-plan")]

public class WeeklyPlanController(
IWeeklyPlanService weeklyPlanManager,
IWeeklyPlanAuthorizationService weeklyPlanAuthorizationService
) : ControllerBase
{
    
     [HttpGet]
     [ProducesResponseType(typeof(DatePagedResult<GetAllPlansPagedBetweenDatesResponse>), StatusCodes.Status200OK)]
     public async Task<IActionResult> GetSelectedPlans([FromQuery] GetSelectedPlansRequest request)
     {
         if (!User.TryGetUserId(out var userId))
             return Unauthorized();
         var result = await weeklyPlanManager.GetAllPlansPagedBetweenDates(userId, request);
         return Ok(result.Value);
     }
    
     [HttpPatch]
     public async Task<IActionResult> UpdatePlan([FromBody] UpdatePlanRequest updatePlanRequest)
     {
         if (!User.TryGetUserId(out var userId))
             return Unauthorized();
         if (await weeklyPlanAuthorizationService.IsAuthorized(userId, updatePlanRequest.Id))
         {
             return Forbid();
         }


         var result = await weeklyPlanManager.UpdatePlan(userId, updatePlanRequest);
         if (result.IsFailure)
         {
             return NotFound(result.Errors);
         }
         return Ok(result);
     }

     [HttpPatch("toggle-status")]
     public async Task<IActionResult> UpdateStatus([FromQuery] Guid planId)
     {
         if (!User.TryGetUserId(out var userId))
         {
             return Unauthorized();
         }
         if (await weeklyPlanAuthorizationService.IsAuthorized(userId, planId))
         {
             return Forbid();
         }
         var result = await weeklyPlanManager.UpdateStatus(planId);
         return Ok(result);
     }
     
     [HttpPost]
     [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
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
    
     [HttpDelete]
     public async Task<IActionResult> DeletePlanItem([FromQuery] Guid planItemId)
     {
         if (!User.TryGetUserId(out var userId))
             return Unauthorized();
         if (await weeklyPlanAuthorizationService.IsAuthorized(userId, planItemId))
         {
             return Forbid();
         }
         var result = await weeklyPlanManager.DeletePlanItem(userId, planItemId);
         if (result.IsFailure)
         {
             return NotFound(result.Errors);
         }
         return Ok(result);
     }

     [HttpGet("overdue")]
     [ProducesResponseType(typeof(List<GetOverduePlansResponse>), StatusCodes.Status200OK)]
     public async Task<IActionResult> GetOverduePlans([FromQuery] DateTime referenceDate, CancellationToken cancellationToken)
     {
         if (!User.TryGetUserId(out var userId))
             return Unauthorized();

         var result = await weeklyPlanManager.GetAllOverduePlans(userId, referenceDate, cancellationToken);
         return Ok(result);
     }
     
}