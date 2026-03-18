using Microsoft.AspNetCore.Identity;
using Itedoro.Business.Services.RegisterService.Dtos;
using Itedoro.Business.Shared.Result;

namespace Itedoro.Business.Services.RegisterService
{
    public interface IRegisterService
    {
        Task<Result<AuthTokens>> RegisterAsync(RegisterRequestDto request);
    }
}