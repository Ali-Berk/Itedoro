using Microsoft.AspNetCore.Mvc;
using Itedoro.Api.Extensions;
using Itedoro.Application.Services.UserServices.Dtos.Requests;
using Itedoro.Application.Services.UserServices.Dtos.Responses;
using Itedoro.Application.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Itedoro.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(
    IUserService userService
) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(GetMeResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();

        var result = await userService.GetMeAsync(userId, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("me")]
    [ProducesResponseType(typeof(GetMeResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> PatchMe([FromBody] UpdateMeRequest request)
    {
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();
        var result = await userService.UpdateMeAsync(userId, request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("me/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PatchPassword([FromBody] UpdatePasswordRequest request)
    {
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();
        var result = await userService.UpdatePasswordAsync(userId, request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("me")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteMe()
    {
        if (!User.TryGetUserId(out Guid userId))
            return Unauthorized();
        var result = await userService.DeleteMeAsync(userId);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("{userName}")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser([FromRoute] string userName, CancellationToken cancellationToken)
    {
        var result = await userService.GetUserAsync(userName, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        var result = await userService.HardDeleteUserAsync(userId);
        if (result.IsFailure)
        {
            return NotFound();
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{userId}/role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PatchUsersRole([FromRoute] Guid userId,[FromBody] UpdateRoleRequest role)
    {
        var result = await userService.UpdateUserRoleAsync(userId, role);
        if (result.IsFailure)
        {
            return NotFound();
        }
        
        //TODO: Value dönülecek
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PatchUser([FromRoute] Guid userId, [FromBody] UpdateUserRequest request)
    {
        var result = await userService.UpdateUserAsync(request);
        if (result.IsFailure)
        {
            return BadRequest();
        }

        return Ok(result.Value);
    }
}