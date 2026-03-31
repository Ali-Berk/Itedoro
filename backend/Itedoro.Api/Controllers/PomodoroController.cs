using Itedoro.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Itedoro.Business.Services.PomodoroService.Dtos;
using Itedoro.Business.Services.PomodoroService.Dtos.Responses;
using Itedoro.Business.Services.PomodoroService.Interfaces;

namespace Itedoro.Api.Controllers;

[Authorize]
[ApiController]
[Route("pomodoro")]
public class PomodoroController(
    IPomodoroService pomodoroService,
    IPomodoroAuthorizationService pomodoroAuthService
) : ControllerBase
{
    //WARN: frontend her child bitiminde backende istek atıp güncellemelidir.
    [HttpPost]
    [ProducesResponseType(typeof(CreatePomodoroResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> StartPomodoro([FromBody] PomodoroPreferencesDto request)
    {
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();
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
    [ProducesResponseType(typeof(PausePomodororoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Pause(Guid parentPomodoroSessionId)
    {
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();
        var canUpdate = await pomodoroAuthService.IsUserTrue(userId, parentPomodoroSessionId);
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
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();
        var canUpdate = await pomodoroAuthService.IsUserTrue(userId, parentPomodoroSessionId);
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
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();
        var canUpdate = await pomodoroAuthService.IsUserTrue(userId, parentPomodoroSessionId);
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
    [ProducesResponseType(typeof(GetPomodoroHistoryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory()
    {
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();
        
        var result = await pomodoroService.GetAllSessionsAsync(userId);
        if (result.IsFailure)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("{parentPomodoroSessionId}/skip-break/{childPomodoroSessionId}")]
    [ProducesResponseType(typeof(SkipBreakResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SkipBreak(Guid parentPomodoroSessionId, Guid childPomodoroSessionId )
    {
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();
        
        var canUpdate = await pomodoroAuthService.IsUserTrue(userId, parentPomodoroSessionId);
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
        if (!User.TryGetUserId(out Guid userId))
        {
            return Unauthorized();
        }
        bool canDelete = await pomodoroAuthService.IsUserTrue(userId, parentPomodoroSessionId);
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