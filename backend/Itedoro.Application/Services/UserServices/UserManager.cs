using Itedoro.Application.Services.UserServices.Dtos.Requests;
using Itedoro.Application.Services.UserServices.Dtos.Responses;
using Itedoro.Application.Services.UserServices.Interfaces;
using Itedoro.Application.Common.Shared.Results;
using Itedoro.Application.Repositories;
using Itedoro.Application.Services.UserServices.Mappers;
using Itedoro.Application.Services.Utils;
using Itedoro.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Itedoro.Domain.Entities.UserStats;

namespace Itedoro.Application.Services.UserServices
{
    public class UserManager(
        IUserRepository repository,
        IPasswordHasher<User> hasher,
        IUserWeekStatRepository weekStat,
        IUserTotalStatRepository totalStat,
        IRoleRepository roleRepository
        ): IUserService
    {
        public async Task<Result<GetMeResponse>> GetMeAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result<GetMeResponse>.Failure("User Not Found");
            }
            var weekId = DateHelper.GetWeekId();
            var weeklyStats = await weekStat.GetByUserAndWeekAsync(user.Id, weekId) ?? new UserWeekStat(0,0,0, 0, weekId, userId);
            var totalStats = await totalStat.GetByUserIdAsync(userId) ?? new UserTotalStat(0,0,userId);
            var mappedUser = user.GetMeResponseMapper(weeklyStats, totalStats);
            return Result<GetMeResponse>.Success(mappedUser);
        }

        public async Task<Result<UpdateMeResponse>> UpdateMeAsync(Guid userId, UpdateMeRequest request)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result<UpdateMeResponse>.Failure("User not exist");
            }

            user.UpdateProfile(request.Name, request.Surname);
            await repository.SaveAsync();
            var result = user.UpdateMeResponseMapper();
            return Result<UpdateMeResponse>.Success(result);
        }

        //TODO: Eposta
        public async Task<Result> UpdatePasswordAsync(Guid userId, UpdatePasswordRequest request)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure("User Not Found");
            }

            var isVerified = hasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
            if (request.NewPassword == request.CurrentPassword || (isVerified is PasswordVerificationResult.Failed))
            {
                return Result.Failure("Current Password Mismatch.");
            }
            
            var newPasswordHash = hasher.HashPassword(user, request.NewPassword);
            
            user.UpdatePasswordHash(newPasswordHash);
            await repository.SaveAsync();
            return Result.Success();
        }

        //TODO: Eposta
        public async Task<Result> DeleteMeAsync(Guid userId)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure("User Not Found");
            }
            user.SoftDelete();
            await repository.SaveAsync();
            return Result.Success();
        }

        public async Task<Result<GetUserResponse>> GetUserAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await repository.GetByUserNameAsync(userName, cancellationToken);
            if (user == null)
            {
                return Result<GetUserResponse>.Failure("User Not Found");
            }

            var mappedUser = user.GetUserResponseMapper();
            return Result<GetUserResponse>.Success(mappedUser);
        }

        public async Task<Result> HardDeleteUserAsync(Guid userId)
        {
            var user = await repository.GetByIdAsync(userId);

            if (user == null)
            {
                return Result.Failure("User Not Found");
            }
            
            repository.Delete(user);
            await repository.SaveAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateUserRoleAsync(Guid userId, UpdateRoleRequest request)
        {
            var user = await repository.GetByIdAsync(userId);
            var role = await roleRepository.GetByNameAsync(request.Role);
            if (user == null || role == null)
            {
                return Result.Failure("Something wrong happen.");
            }

            user.AssignRole(role.Id);
            await repository.SaveAsync();
            return Result.Success();
        }

        public async Task<Result<UpdateUserResponse>> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result<UpdateUserResponse>.Failure("User Not Found");
            }
            user.UpdateProfile(request.Name, request.Surname);
            await repository.SaveAsync();
            var result = user.UpdateUserResponseMapper();
            return Result<UpdateUserResponse>.Success(result);
        }
    }
}