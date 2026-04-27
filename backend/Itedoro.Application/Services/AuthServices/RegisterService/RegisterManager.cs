using Microsoft.AspNetCore.Identity;
using Itedoro.Domain.Entities.Users;
using Itedoro.Application.Common.Shared.Results;
using Itedoro.Application.Services.AuthServices.RegisterService.Interfaces;
using Itedoro.Application.Services.AuthServices.Dtos.Requests;
using Itedoro.Application.Repositories;

namespace Itedoro.Application.Services.AuthServices.RegisterService;
public class RegisterManager(
    IAuthRepository repository,
    IPasswordHasher<User> passwordHasher
) : IRegisterService
{

    public async Task<Result> RegisterAsync(RegisterRequest request)
    {
        
        if (await repository.CheckIfUserExistsAsync(request.Username, request.Email))
        {
            return Result.Failure("User already exists.");
        }
        
        var user = new User(request.Username, request.Email, "placeholder", request.Name, request.Surname);
        
        string hashedPassword = passwordHasher.HashPassword(user, request.Password);
        user.UpdatePasswordHash(hashedPassword);

        await repository.AddUserAsync(user);
        await repository.SaveChangesAsync();
        return Result.Success();
    }
}
