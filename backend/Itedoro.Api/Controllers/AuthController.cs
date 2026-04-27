using Microsoft.AspNetCore.Mvc;
using Itedoro.Application.Services.AuthServices.Dtos.Requests;
using Itedoro.Application.Services.AuthServices.Dtos.Responses;
using Itedoro.Application.Services.AuthServices.LoginService.Interfaces;
using Itedoro.Application.Services.AuthServices.RegisterService.Interfaces;
using Itedoro.Application.Services.AuthServices.TokenService.Interfaces;

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
    //TODO: Contract oluştur.
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
    
    //TODO: Lougout ekle
}
