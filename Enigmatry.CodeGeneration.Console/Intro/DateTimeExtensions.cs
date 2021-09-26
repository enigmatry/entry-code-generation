using System;

namespace Enigmatry.CodeGeneration.Console.Intro
{
    internal static class DateTimeExtensions
    {
        public static bool IsWeekend(this DateTime date) =>
            date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;

        public static bool IsNewYearsEve(this DateTime date) =>
            date.ToString("dd/MM") == "31/12";

        public static bool IsBirthDay(this DateTime date) =>
            DateTime.Now.ToString("dd/MM") == date.ToString("dd/MM");

        public static bool IsLunchTime(this DateTime date) =>
            date.Hour >= 12 && date.Hour <= 13;

        public static bool IsMorning(this DateTime date) =>
            date.Hour >= 6 && date.Hour <= 10;

        public static bool IsEvening(this DateTime date) =>
            date.Hour >= 20 && date.Hour <= 23;

        public static bool IsNight(this DateTime date) =>
            date.Hour >= 0 && date.Hour <= 6;

        public static bool IsFriday13th(this DateTime date) =>
            date.IsFriday() && date.Day == 13;

        public static bool IsFriday(this DateTime date) =>
            date.DayOfWeek == DayOfWeek.Friday;

        public static bool IsMonday(this DateTime date) =>
            date.DayOfWeek == DayOfWeek.Monday;
    }
}
