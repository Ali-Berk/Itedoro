using Itedoro.Application.Common.Shared.Results;
using Itedoro.Application.Services.AuthServices.Dtos.Requests;

namespace Itedoro.Application.Services.AuthServices.RegisterService.Interfaces;

public interface IRegisterService
{
    Task<Result> RegisterAsync(RegisterRequest request);
}
