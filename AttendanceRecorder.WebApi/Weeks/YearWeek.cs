using System.Globalization;

namespace AttendanceRecorder.WebApi.Weeks;

public struct YearWeek
{
    public YearWeek(int year, int number)
    {
        Year = year;
        Number = number;
    }

    public int Year { get; }

    public int Number { get; }

    public DateOnly FirstDate => DateOnly.FromDateTime(ISOWeek.ToDateTime(Year, Number, DayOfWeek.Monday));

    public DateOnly LastDate => DateOnly.FromDateTime(ISOWeek.ToDateTime(Year, Number, DayOfWeek.Sunday));

    public static YearWeek FromDate(DateOnly date)
    {
        var dateTime = date.ToDateTime(TimeOnly.MinValue);
        var year = ISOWeek.GetYear(dateTime);
        var number = ISOWeek.GetWeekOfYear(dateTime);
        return new YearWeek(year, number);
    }
}
