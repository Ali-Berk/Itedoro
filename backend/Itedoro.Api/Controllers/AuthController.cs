using Itedoro.Business.Services.RegisterService;
using Itedoro.Business.Services.RegisterService.Dtos;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Business.Services.LoginService;
namespace Itedoro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRegisterService _registerService;
    private readonly ILoginService _loginService;
    public AuthController(IRegisterService registerService, ILoginService loginService)
    {
        _registerService = registerService;
        _loginService = loginService;
    }

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


    [HttpGet("test")]
    public string Test()
    {
        return "Controller calisiyor, veritabanina giden yol acik!";
    }
}