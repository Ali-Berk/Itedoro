using Microsoft.AspNetCore.Mvc;
using Itedoro.Api.Extensions;
using Itedoro.Api.Services.CurrentUser;
using Itedoro.Application.Services.UserServices.Dtos.Requests;
using Itedoro.Application.Services.UserServices.Dtos.Responses;
using Itedoro.Application.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Itedoro.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(
    IUserService userService,
    ICurrentUserService currentUserService
) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(GetMeResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMeAsync(CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();

        var userId = currentUserService.UserId.Value;
        var result = await userService.GetMeAsync(userId, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("me")]
    [ProducesResponseType(typeof(GetMeResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateMeAsync([FromBody] UpdateMeRequest request)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();
        var userId = currentUserService.UserId.Value;
        var result = await userService.UpdateMeAsync(userId, request);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("me/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordRequest request)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();
        var userId = currentUserService.UserId.Value;
        var result = await userService.UpdatePasswordAsync(userId, request);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("me")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteMeAsync()
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
            return Unauthorized();
        var userId = currentUserService.UserId.Value;
        var result = await userService.DeleteMeAsync(userId);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("{userName}")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserAsync([FromRoute] string userName, CancellationToken cancellationToken)
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
    public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid userId)
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
    public async Task<IActionResult> UpdateUserRoleAsync([FromRoute] Guid userId,[FromBody] UpdateRoleRequest role)
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid userId, [FromBody] UpdateUserRequest request)
    {
        var result = await userService.UpdateUserAsync(userId, request);
        if (result.IsFailure)
        {
            return BadRequest();
        }

        return Ok(result.Value);
    }
}