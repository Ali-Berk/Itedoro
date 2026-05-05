using Itedoro.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Itedoro.Application.Services.AuthServices.Dtos.Requests;
using Itedoro.Application.Services.AuthServices.Dtos.Responses;
using Itedoro.Application.Services.AuthServices.LoginService.Interfaces;
using Itedoro.Application.Services.AuthServices.RegisterService.Interfaces;
using Itedoro.Application.Services.AuthServices.TokenService.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Itedoro.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    ITokenService tokenService,
    ILoginService loginService,
    IRegisterService registerService,
    IConfiguration configuration
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await registerService.RegisterAsync(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Created();
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await loginService.LoginAsync(request);
        if(result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        
        Response.SetRefreshTokenCookie(result.Value!.RefreshToken, configuration);
        return Ok(result.Value);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }
        var result = await tokenService.RefreshAsync(refreshToken);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await loginService.LogoutAsync();
        if (result.IsFailure)
        {
            return BadRequest();
        }

        Response.DeleteRefreshTokenCookie();
        return Ok();
    }
}
