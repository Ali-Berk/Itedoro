using Itedoro.Application.Common.Shared.Results;

namespace Itedoro.Application.Services.UserServices.Errors;

public static class UserErrors
{
    public static Error PasswordsMissmatch => new("PASSWORDS_MISSMATCH", "Passwords do not match");
}