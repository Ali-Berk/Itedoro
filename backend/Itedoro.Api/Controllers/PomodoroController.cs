using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Itedoro.Business.Services.PomodoroService;
using Itedoro.Business.Services.PomodoroService.Dtos;

namespace Itedoro.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PomodoroController(
    IPomodoroService pomodoroService
) : ControllerBase
{
    //TODO: Geçmiş pomodoroların listelenmesi için bir endpoint oluştur.

    [HttpPost("start")]
    public async Task<IActionResult> StartPomodoro([FromBody] PomodoroPreferencesDto request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(userIdString, out Guid userId);
        var result = await pomodoroService.CreateSessionAsync(userId, request);

        if (result.IsSuccess && result.Value is not null)
        {
            return Ok(new { parentId = result.Value.Id });
        }

        return BadRequest(new
        {
            message = "Pomodoro session could not be created.",
            errors = result.Errors
        });
    }
    //DONE: Pause ve resume için isteği urlden al
    [HttpPost("pause/{parentPomodoroSessionId}")]
    public async Task<IActionResult> Pause(Guid parentPomodoroSessionId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(userIdString, out Guid userId);
        var result = await pomodoroService.PauseSessionAsync(userId, parentPomodoroSessionId);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }

    [HttpPost("resume/{parentPomodoroSessionId}")]
    public async Task<IActionResult> Resume(Guid parentPomodoroSessionId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(userIdString, out Guid userId);
        var result = await pomodoroService.ResumeSessionAsync(userId, parentPomodoroSessionId);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }

    [HttpPost("stop/{parentPomodoroSessionId}")]
    public async Task<IActionResult> Stop(Guid parentPomodoroSessionId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(userIdString, out Guid userId);
        var result = await pomodoroService.StopSessionAsync(userId, parentPomodoroSessionId);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }
}