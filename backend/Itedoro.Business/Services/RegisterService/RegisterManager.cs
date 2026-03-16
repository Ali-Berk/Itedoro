using Microsoft.AspNetCore.Identity;
using Itedoro.Data.Entities.Users;
using Itedoro.Data;
using Itedoro.Business.Services.RegisterService.Dtos;
using Itedoro.Business.Services.UserServices;

namespace Itedoro.Business.Services.RegisterService;
public class RegisterManager : IRegisterService
{
    private readonly IUserService _userManager;
    public RegisterManager(IUserService userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterRequestDto request)
    {        
        
        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname,
        };

        return await _userManager.CreateAsync(newUser, request.Password);
        
    }
}