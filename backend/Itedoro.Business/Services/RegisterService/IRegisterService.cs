using Microsoft.AspNetCore.Identity;
using Itedoro.Business.Services.RegisterService.Dtos;

namespace Itedoro.Business.Services.RegisterService
{
    public interface IRegisterService
    {
        Task<IdentityResult> RegisterAsync(RegisterRequestDto request);
    }
}