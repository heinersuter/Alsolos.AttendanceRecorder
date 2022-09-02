using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace AttendanceRecorder.WebApi.Weeks;

public class WeeksService
{
    private readonly FileSystemOptions _fileSystemOptions;

    public WeeksService(IOptions<FileSystemOptions> fileSystemOptions)
    {
        _fileSystemOptions = fileSystemOptions.Value;
    }

    public IEnumerable<YearWeek> FindAvailableWeeks()
    {
        var dates = FindAvailableDates();
        var weeks = dates
            .Select(YearWeek.FromDate)
            .Distinct();

        return weeks;
    }

    private IEnumerable<Date> FindAvailableDates()
    {
        var years = Directory.EnumerateDirectories(_fileSystemOptions.LocalDirectory)
            .Where(path => Regex.IsMatch(Path.GetFileName(path), @"^\d{4}$"))
            .ToDictionary(path => int.Parse(Path.GetFileName(path)));

        foreach (var year in years)
        {
            var months = Directory.EnumerateDirectories(year.Value)
                .Where(path => Regex.IsMatch(Path.GetFileName(path), @"^\d{2}$"))
                .ToDictionary(path => int.Parse(Path.GetFileName(path)));

            foreach (var month in months)
            {
                var days = Directory.EnumerateFiles(month.Value)
                    .Where(path => Regex.IsMatch(Path.GetFileName(path), @"^\d{2}\.att$"))
                    .ToDictionary(path => int.Parse(Path.GetFileNameWithoutExtension(path)));

                foreach (var day in days)
                {
                    yield return new Date(year.Key, month.Key, day.Key);
                }
            }
        }
    }
}

public struct YearWeek
{
    public YearWeek(int year, int number)
    {
        Year = year;
        Number = number;
    }

    public int Year { get; }
    public int Number { get; }
    public Date FirstDate => Date.FromDateTime(ISOWeek.ToDateTime(Year, Number, DayOfWeek.Monday));
    public Date LastDate => Date.FromDateTime(ISOWeek.ToDateTime(Year, Number, DayOfWeek.Sunday));

    public static YearWeek FromDate(Date date)
    {
        var dateTime = date.ToDateTime();
        var year = ISOWeek.GetYear(dateTime);
        var number = ISOWeek.GetWeekOfYear(dateTime);
        return new YearWeek(year, number);
    }
}

public struct Date
{
    public Date(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public int Year { get; }
    public int Month { get; }
    public int Day { get; }

    public static Date FromDateTime(DateTime dateTime)
    {
        return new Date(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public DateTime ToDateTime()
    {
        return new DateTime(Year, Month, Day);
    }
}
