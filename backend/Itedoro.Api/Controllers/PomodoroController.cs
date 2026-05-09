using Itedoro.Api.Extensions;
using Itedoro.Api.Services.CurrentUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Itedoro.Application.Services.PomodoroService.Dtos.Requests;
using Itedoro.Application.Services.PomodoroService.Dtos.Responses;
using Itedoro.Application.Services.PomodoroService.Interfaces;
using Itedoro.Application.Common.Models;

namespace Itedoro.Api.Controllers;

[Authorize]
[ApiController]
[Route("pomodoro")]
public class PomodoroController(
    IPomodoroService pomodoroService,
    IPomodoroAuthorizationService pomodoroAuthService,
    ICurrentUserService currentUserService
) : ControllerBase
{
    //WARN: frontend her child bitiminde backende istek atıp güncellemelidir.
    [HttpPost]
    [ProducesResponseType(typeof(CreatePomodoroResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> StartPomodoro([FromBody] CreatePomodoroRequest request)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();
        var userId = currentUserService.UserId.Value;
        var result = await pomodoroService.CreateSessionAsync(userId, request);

        if (result.IsSuccess && result.Value is not null)
        {
            return Ok(result.Value);
        }

        return BadRequest(new
        {
            message = "Pomodoro session could not be created.",
            errors = result.Errors
        });
    }
    [HttpPatch("pause/{parentPomodoroSessionId}")]
    [ProducesResponseType(typeof(PausePomodoroResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Pause(Guid parentPomodoroSessionId)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();
        var userId = currentUserService.UserId.Value;
        var canUpdate = await pomodoroAuthService.IsOwnedByUserAsync(userId, parentPomodoroSessionId);
        if (!canUpdate)
        {
            return Forbid();
        }
        var result = await pomodoroService.PauseSessionAsync(userId, parentPomodoroSessionId);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("resume/{parentPomodoroSessionId}")]
    [ProducesResponseType(typeof(ResumePomodoroResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Resume(Guid parentPomodoroSessionId)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();
        var userId = currentUserService.UserId.Value;
        var canUpdate = await pomodoroAuthService.IsOwnedByUserAsync(userId, parentPomodoroSessionId);
        if (!canUpdate)
        {
            return Forbid();
        }
        var result = await pomodoroService.ResumeSessionAsync(userId, parentPomodoroSessionId);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("stop/{parentPomodoroSessionId}")]
    [ProducesResponseType(typeof(StopPomodoroResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Stop(Guid parentPomodoroSessionId)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();
        var userId = currentUserService.UserId.Value;
        var canUpdate = await pomodoroAuthService.IsOwnedByUserAsync(userId, parentPomodoroSessionId);
        if (!canUpdate)
        {
            return Forbid();
        }
        var result = await pomodoroService.StopSessionAsync(userId, parentPomodoroSessionId);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GetPomodoroHistoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory([FromQuery] GetPomodoroHistoryRequest request)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();
        var userId = currentUserService.UserId.Value;
        var result = await pomodoroService.GetPagedSessionsAsync(userId, request);
        return Ok(result.Value);
    }

    [HttpPatch("{parentPomodoroSessionId}/skip-break/{childPomodoroSessionId}")]
    [ProducesResponseType(typeof(SkipBreakResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SkipBreak(Guid parentPomodoroSessionId, Guid childPomodoroSessionId )
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();
        var userId = currentUserService.UserId.Value;

        var canUpdate = await pomodoroAuthService.IsOwnedByUserAsync(userId, parentPomodoroSessionId);
        if (!canUpdate)
        {
            return Forbid();
        }
        var result = await pomodoroService.SkipBreakAsync(parentPomodoroSessionId, childPomodoroSessionId);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{parentPomodoroSessionId:guid}")]
    public async Task<IActionResult> DeletePomodoro(Guid parentPomodoroSessionId)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
        {
            return Unauthorized();
        }
        var userId = currentUserService.UserId.Value;
        bool canDelete = await pomodoroAuthService.IsOwnedByUserAsync(userId, parentPomodoroSessionId);
        if (!canDelete)
        {
            return Forbid();
        }

        var result = await pomodoroService.DeleteSessionAsync(parentPomodoroSessionId);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }
}
