using Itedoro.Business.Services.PomodoroService.Dtos;
using Itedoro.Business.Services.PomodoroService.Dtos.Requests;
using Itedoro.Business.Services.PomodoroService.Dtos.Responses;
using Itedoro.Business.Shared.Result;
using Itedoro.Data.Shared;

namespace Itedoro.Business.Services.PomodoroService.Interfaces;

public interface IPomodoroService
{
    Task<Result<CreatePomodoroResponse>> CreateSessionAsync(Guid userId, PomodoroPreferencesDto dto);

    Task<Result<PausePomodoroResponse>> PauseSessionAsync(Guid userId, Guid parentId);
    Task<Result<ResumePomodoroResponse>> ResumeSessionAsync(Guid userId, Guid parentId);
    Task<Result<StopPomodoroResponse>> StopSessionAsync(Guid userId, Guid parentId);
    Task<Result<PagedResult<GetPomodoroHistoryResponse>>> GetPagedSessionsAsync(Guid userId, GetPomodoroHistoryRequest dto);
    Task<Result<SkipBreakResponse>> SkipBreakAsync(Guid parentId, Guid childId);
    Task<Result> DeleteSessionAsync(Guid parentId);
}

