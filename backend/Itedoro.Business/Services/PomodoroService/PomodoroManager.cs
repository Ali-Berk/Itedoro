using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Business.Services.PomodoroService.Dtos;
using Itedoro.Data;
using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.Utils;

namespace Itedoro.Business.Services.PomodoroService;

public class PomodoroManager(
    ItedoroDbContext dbContext,
    PomodoroPlanGenerator planGenerator
) : IPomodoroService
{
//TODO: Aktif olan 1 adet pomodoro olması gerekli. Kontrolünü sağla.
//TODO: Controller için validation ayarla.
//TODO: Pause ve Resume ekle.(Not: Bitiş süresini ilerletmen gerekecek.)
//TODO: Childların çalışma sırası için bir order ekle.

    public async Task<Result<ParentSession>> CreateSessionAsync(Guid userId, PomodoroPreferencesDto dto)
    {
        var childPlans = planGenerator.Generate(dto);
        var newSession = new ParentSession(out Guid parentId, userId, dto.TotalMinutes);
        var parent = await dbContext.ParentSessions.AddAsync(newSession);
        foreach (var plan in childPlans)
        {
            var child = new ChildSession(parentId, plan.Duration, plan.Type);
            dbContext.ChildSessions.Add(child);
        }
        await dbContext.SaveChangesAsync();
        return Result<ParentSession>.Success(newSession);
    }
}