using System;
using System.Threading.Tasks;
using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Business.Services.PomodoroService.Dtos;
using Itedoro.Business.Shared.Result; 

namespace Itedoro.Business.Services.PomodoroService;

public interface IPomodoroService
{
    Task<Result<ParentSession>> CreateSessionAsync(Guid userId, PomodoroPreferencesDto dto);

    Task<Result> PauseSessionAsync(Guid userId, Guid parentId);
    Task<Result> ResumeSessionAsync(Guid userId, Guid parentId);

    // Task<Result> StopSessionAsync(Guid sessionId);
    // Task<Result<ParentSession>> GetActiveSessionAsync(Guid userId);
}