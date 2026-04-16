using Itedoro.Application.Services.PomodoroService.Dtos.Requests;
using Itedoro.Domain.Enums;

namespace Itedoro.Application.Services.Utils;
public class PomodoroPlanGenerator
{
    //TODO: Son child Mola veya Uzun Mola olamaz.
    public List<(int Duration, PomodoroType Type, int Order)> Generate(CreatePomodoroRequest prefs)
    {
        var planList = new List<(int Duration, PomodoroType Type, int Order)>();
        int remainingMinutes = prefs.TotalMinutes;
        int workSessionCount = 0;
        int order = 0;
        while (remainingMinutes > 0)
        {
            order++;
            int currentWork = Math.Min(remainingMinutes, prefs.WorkMinutes);
            planList.Add((currentWork, PomodoroType.Work, order));
            remainingMinutes -= currentWork;
            workSessionCount++;

            if (remainingMinutes <= 0) break;

            bool isLongBreak = workSessionCount % prefs.LongBreakInterval == 0;
            int breakLimit = isLongBreak ? prefs.LongBreakMinutes : prefs.ShortBreakMinutes;
            PomodoroType breakType = isLongBreak ? PomodoroType.LongBreak : PomodoroType.ShortBreak;

            int currentBreak = Math.Min(remainingMinutes, breakLimit);
            order++;
            planList.Add((currentBreak, breakType, order));
            remainingMinutes -= currentBreak;
        }

        return planList;
    }
}