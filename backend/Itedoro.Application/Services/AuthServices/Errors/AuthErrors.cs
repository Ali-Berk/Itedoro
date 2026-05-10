using Itedoro.Application.Common.Shared.Results;

namespace Itedoro.Application.Services.AuthServices.Errors;

public class AuthErrors
{
        public static Error UserExist => new("USER_EXIST", "User already exist.");
        public static Error InvalidCredentials = new("INVALID_CREDENTIALS", "Invalid username/email or password.");
        public static Error InvalidRefreshToken = new("INVALID_REFRESH_TOKEN", "Refresh token is either expired or invalid.");
        public static Error InvalidLoginHandle = new("INVALID_LOGIN_HANDLE", "Invalid login handle.");
}