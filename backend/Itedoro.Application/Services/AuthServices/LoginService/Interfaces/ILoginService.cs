using Itedoro.Application.Common.Shared.Result;
using Itedoro.Application.Services.AuthServices.Dtos.Requests;
using Itedoro.Application.Services.AuthServices.Dtos.Responses;

namespace Itedoro.Application.Services.AuthServices.LoginService.Interfaces;

public interface ILoginService
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
}
