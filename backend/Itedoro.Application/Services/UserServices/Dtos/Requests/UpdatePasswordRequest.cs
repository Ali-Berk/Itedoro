namespace Itedoro.Application.Services.UserServices.Dtos.Requests;

public record UpdatePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string NewPasswordConfirm);