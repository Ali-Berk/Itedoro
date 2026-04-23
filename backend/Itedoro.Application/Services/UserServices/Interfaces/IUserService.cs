using Itedoro.Application.Services.UserServices.Dtos.Requests;
using Itedoro.Application.Services.UserServices.Dtos.Responses;
using Itedoro.Application.Common.Shared.Results;
namespace Itedoro.Application.Services.UserServices.Interfaces
{
    public interface IUserService
    {
        Task<Result<GetMeResponse>> GetMeAsync(Guid userId, CancellationToken cancellationToken);

        Task<Result<UpdateMeResponse>> UpdateMeAsync(Guid userId, UpdateMeRequest request);

        Task<Result> UpdatePasswordAsync(Guid userId, UpdatePasswordRequest request);

        Task<Result> DeleteMeAsync(Guid userId);

        Task<Result<GetUserResponse>> GetUserAsync(string userName, CancellationToken cancellationToken);

        Task<Result> HardDeleteUserAsync(Guid userId);

        Task<Result> UpdateUserRoleAsync(Guid userId, UpdateRoleRequest request);

        Task<Result<UpdateUserResponse>> UpdateUserAsync(Guid userId, UpdateUserRequest request);
    }
}