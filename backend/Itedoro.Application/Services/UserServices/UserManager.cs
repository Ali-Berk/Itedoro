using Itedoro.Application.Services.UserServices.Dtos.Requests;
using Itedoro.Application.Services.UserServices.Dtos.Responses;
using Itedoro.Application.Services.UserServices.Interfaces;
using Itedoro.Application.Common.Shared.Results;

namespace Itedoro.Application.Services.UserServices
{
    public class UserManager : IUserService
    {
        public async Task<Result<GetMeResponse>> GetMeAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<UpdateMeResponse>> UpdateMeAsync(Guid userId, UpdateMeRequest request)
        {
            throw new NotImplementedException();
        }

        //TODO: Eposta
        public async Task<Result> UpdatePasswordAsync(Guid userId, UpdatePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        //TODO: Eposta
        public async Task<Result> DeleteMeAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<GetUserResponse>> GetUserAsync(string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> HardDeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateUserRoleAsync(Guid userId, UpdateRoleRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<UpdateUserResponse>> UpdateUserAsync(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}