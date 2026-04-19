using Itedoro.Application.Services.PomodoroService.Dtos.Requests;
using Itedoro.Application.Services.PomodoroService.Dtos.Responses;
using Itedoro.Application.Common.Shared.Results;
using Itedoro.Application.Common.Models;

namespace Itedoro.Application.Services.PomodoroService.Interfaces;

public interface IPomodoroService
{
    Task<Result<CreatePomodoroResponse>> CreateSessionAsync(Guid userId, CreatePomodoroRequest dto);

    Task<Result<PausePomodoroResponse>> PauseSessionAsync(Guid userId, Guid parentId);
    Task<Result<ResumePomodoroResponse>> ResumeSessionAsync(Guid userId, Guid parentId);
    Task<Result<StopPomodoroResponse>> StopSessionAsync(Guid userId, Guid parentId);
    Task<Result<PagedResult<GetPomodoroHistoryResponse>>> GetPagedSessionsAsync(Guid userId, GetPomodoroHistoryRequest dto);
    Task<Result<SkipBreakResponse>> SkipBreakAsync(Guid parentId, Guid childId);
    Task<Result> DeleteSessionAsync(Guid parentId);
}
