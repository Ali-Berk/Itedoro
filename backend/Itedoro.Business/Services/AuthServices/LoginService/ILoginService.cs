using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;
using Itedoro.Business.Services.AuthServices.Dtos.Responses;

namespace Itedoro.Business.Services.AuthServices.LoginService
{
    public interface ILoginService
    {
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
    }
    
}