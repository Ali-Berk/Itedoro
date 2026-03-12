using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Business.Services.UserServices;
using Itedoro.Data.Entities.Users;

namespace Itedoro.Business.Services.LoginService;

public class UsernameLoginStrategy : ILoginStrategy
{
    private readonly IUserService _userManager;
    public UsernameLoginStrategy(IUserService userManager)
    {
        _userManager = userManager;
    }
    public bool CanHandle(LoginRequestDto request) => !request.LoginHandle.Contains("@");
    public async Task<User?> LoginAsync(LoginRequestDto request)
    {
        return await _userManager.GetByUsernameAsync(request.LoginHandle);
    }
}