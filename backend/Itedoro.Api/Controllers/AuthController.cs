using Itedoro.Business.Services.RegisterService;
using Itedoro.Business.Services.RegisterService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Business.Services.LoginService;
using Itedoro.Business.Services.TokenService;
using Itedoro.Business.Shared.Result;

namespace Itedoro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    ITokenService tokenService,
    ILoginService loginService,
    IRegisterService registerService
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var result = await registerService.RegisterAsync(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await loginService.LoginAsync(request);
        if(result?.IsSuccess == true)
        {
            return Ok(result);
        }

        return BadRequest(result?.Errors);
    }

    //TODO:Refresh için validation ekle.
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var result = await tokenService.RefreshAsync(refreshToken);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result?.Errors);
    }
}
