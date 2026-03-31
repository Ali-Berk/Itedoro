using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;

namespace Itedoro.Business.Services.AuthServices.RegisterService
{
    public interface IRegisterService
    {
        Task<Result> RegisterAsync(RegisterRequest request);
    }
}