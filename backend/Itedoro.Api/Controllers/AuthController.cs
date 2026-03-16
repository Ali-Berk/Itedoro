using Itedoro.Business.Services.RegisterService;
using Itedoro.Business.Services.RegisterService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Business.Services.LoginService;
using Itedoro.Data.Entities.Users;
using Itedoro.Business.Services.TokenService;
using Microsoft.EntityFrameworkCore;
using Itedoro.Data;

namespace Itedoro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    ITokenService _tokenService,
    ILoginService _loginService,
    IRegisterService _registerService,
    ItedoroDbContext context
) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _registerService.RegisterAsync(request);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _loginService.LoginAsync(request);
        if(result?.IsSuccess == true)
        {
            return Ok(result);
        }

        return BadRequest(new {message = "Kullanıcı girişi onaylanmadı."});
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var exist = await context.RefreshTokens
            .Include(t => t.user).Include(t => t.user.Role)
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (exist == null || exist.IsExpired)
        {
            return BadRequest("Invalid refresh token.");
        }
        var accessToken = _tokenService.GenereteAccessToken(exist.user);
        return Ok(accessToken);
    }


    [HttpGet("test")]
    public string Test()
    {
        return "Controller calisiyor";
    }
}