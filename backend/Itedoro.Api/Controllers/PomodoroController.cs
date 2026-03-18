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

    [HttpGet("test")]
    public string Test()
    {
        return "Pomodoro Controller calisiyor";
    }
}