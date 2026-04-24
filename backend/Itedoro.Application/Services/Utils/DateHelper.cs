using System.Globalization;

namespace Itedoro.Application.Services.Utils;

public static class DateHelper
{
    public static string GetWeekId()
    {
        var now = DateTime.UtcNow;
        var cal = CultureInfo.InvariantCulture.Calendar;
        var week = cal.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        return $"{now.Year}W{week}";
    }
}