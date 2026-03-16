
using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Business.Shared.Result;

namespace Itedoro.Business.Services.LoginService
{
    public interface ILoginService
    {
        Task<Result<AuthTokens>> LoginAsync(LoginRequestDto request);
    }
    
}