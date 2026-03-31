namespace Itedoro.Business.Services.AuthServices.Dtos.Requests;
public record LoginRequest(
    string LoginHandle,
    string Password
);