// FRM.BuisnessLogic/Helper/TimeHelper.cs
using System;

namespace FRM.BuisnessLogic.Helper
{
    public static class TimeHelper
    {
        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.UtcNow - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("{0} {1} назад", years, (years == 1) ? "год" : "лет");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("{0} {1} назад", months, (months == 1) ? "месяц" : "месяцев");
            }
            if (span.Days > 0)
                return String.Format("{0} {1} назад", span.Days, (span.Days == 1) ? "день" : "дней");
            if (span.Hours > 0)
                return String.Format("{0} {1} назад", span.Hours, (span.Hours == 1) ? "час" : "часов");
            if (span.Minutes > 0)
                return String.Format("{0} {1} назад", span.Minutes, (span.Minutes == 1) ? "минуту" : "минут");
            if (span.Seconds > 5)
                return String.Format("{0} секунд назад", span.Seconds);
            if (span.Seconds <= 5)
                return "только что";
            return string.Empty;
        }
    }
}