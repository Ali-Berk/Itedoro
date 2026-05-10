using Itedoro.Application.Common.Shared.Results;

namespace Itedoro.Application.Services.PomodoroService.Errors;

public static class PomodoroErrors
{
    public static Error SessionCannotStopped => new("SESSION_CANNOT_STOPPED", "This session is not active and cannot be stopped.");
                
}