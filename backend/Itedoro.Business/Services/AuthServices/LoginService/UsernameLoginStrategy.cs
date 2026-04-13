using Itedoro.Data.Entities.Users;
using Itedoro.Data.Repositories.Auth.Interfaces;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;

namespace Itedoro.Business.Services.AuthServices.LoginService;

public class UsernameLoginStrategy(
    IAuthRepository repository
    ) : ILoginStrategy
{
    public bool CanHandle(LoginRequest request) => !request.LoginHandle.Contains("@");
    public async Task<User?> LoginAsync(LoginRequest loginRequest)
    {
        return await repository.GetUserByUsernameAsync(loginRequest.LoginHandle);
    }
}