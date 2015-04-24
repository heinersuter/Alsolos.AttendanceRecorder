namespace Alsolos.AttendanceRecorder.Client.Views.Model
{
    using System;
    using System.Globalization;
    using Alsolos.AttendanceRecorder.WebApiModel;

    public struct YearWeek
    {
        private readonly int _year;
        private readonly int _week;

        public YearWeek(int year, int week)
        {
            _year = year;
            _week = week;
        }

        public YearWeek(DateTime date)
        {
            _year = date.Year;
            _week = GetWeekNumber(date);
        }

        public int Year
        {
            get { return _year; }
        }

        public int Week
        {
            get { return _week; }
        }

        public DatePeriod ToDatePeriod()
        {
            var firstDayOfWeek = FirstDateOfWeek();
            return new DatePeriod(
                DatePeriodType.Week,
                string.Format(CultureInfo.InvariantCulture, "{0:D4} - Week {1:D2}", Year, Week),
                new Date(firstDayOfWeek),
                new Date(firstDayOfWeek.AddDays(6)));
        }

        private static int GetWeekNumber(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private DateTime FirstDateOfWeek()
        {
            var jan1 = new DateTime(Year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            var firstThursday = jan1.AddDays(daysOffset);
            var firstWeek = GetWeekNumber(firstThursday);

            var weekNum = Week;
            if (firstWeek <= 1)
            {
                weekNum--;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }
}