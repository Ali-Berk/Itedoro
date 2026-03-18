using Itedoro.Business.Services.PomodoroService.Dtos;

namespace Itedoro.Business.Services.Utils;
public class PomodoroPlanGenerator
{
    public List<(int Duration, PomodoroType Type)> Generate(PomodoroPreferencesDto prefs)
    {
        var planList = new List<(int Duration, PomodoroType Type)>();
        int remainingMinutes = prefs.TotalMinutes;
        int workSessionCount = 0;

        while (remainingMinutes > 0)
        {
            int currentWork = Math.Min(remainingMinutes, prefs.WorkMinutes);
            planList.Add((currentWork, PomodoroType.Work));
            remainingMinutes -= currentWork;
            workSessionCount++;

            if (remainingMinutes <= 0) break;

            bool isLongBreak = workSessionCount % prefs.LongBreakInterval == 0;
            int breakLimit = isLongBreak ? prefs.LongBreakMinutes : prefs.ShortBreakMinutes;
            PomodoroType breakType = isLongBreak ? PomodoroType.LongBreak : PomodoroType.ShortBreak;

            int currentBreak = Math.Min(remainingMinutes, breakLimit);
            planList.Add((currentBreak, breakType));
            remainingMinutes -= currentBreak;
        }

        return planList;
    }
}