using Microsoft.AspNetCore.Mvc;
using Itedoro.Business.Services.AuthServices.TokenService;
using Itedoro.Business.Services.AuthServices.LoginService;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;
using Itedoro.Business.Services.AuthServices.Dtos.Responses;
using Itedoro.Business.Services.AuthServices.RegisterService;

namespace Itedoro.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    ITokenService tokenService,
    ILoginService loginService,
    IRegisterService registerService
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
        if(result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var result = await tokenService.RefreshAsync(refreshToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }
}
