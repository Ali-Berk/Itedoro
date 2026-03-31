using Itedoro.Business.Services.PomodoroService.Dtos;
using Itedoro.Business.Services.PomodoroService.Dtos.Responses;
using Itedoro.Business.Shared.Result; 

namespace Itedoro.Business.Services.PomodoroService.Interfaces;

public interface IPomodoroService
{
    Task<Result<CreatePomodoroResponse>> CreateSessionAsync(Guid userId, PomodoroPreferencesDto dto);

    Task<Result<PausePomodororoResponse>> PauseSessionAsync(Guid userId, Guid parentId);
    Task<Result<ResumePomodoroResponse>> ResumeSessionAsync(Guid userId, Guid parentId);
    Task<Result<StopPomodoroResponse>> StopSessionAsync(Guid userId, Guid parentId);
    Task<Result<List<GetPomodoroHistoryResponse>>> GetAllSessionsAsync(Guid userId);
    Task<Result<SkipBreakResponse>> SkipBreakAsync(Guid parentId, Guid childId);
    Task<Result> DeleteSessionAsync(Guid parentId);
}

