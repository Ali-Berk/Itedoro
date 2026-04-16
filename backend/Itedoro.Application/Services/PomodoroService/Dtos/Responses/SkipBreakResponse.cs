namespace Itedoro.Application.Services.PomodoroService.Dtos.Responses;

public record SkipBreakResponse(
    Guid ParentId,
    Guid SkippedChildId,
    int? NextChildId,
    DateTime NewEndTime);