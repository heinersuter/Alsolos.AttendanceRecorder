using System.Text.RegularExpressions;

namespace AttendanceRecorder.WebApi.FileSystem;

public struct MonthDirectory
{
    public MonthDirectory(int year, int month, string path)
    {
        Year = year;
        Month = month;
        Path = path;
    }

    public int Year { get; }

    public int Month { get; }

    public string Path { get; }

    public static bool IsMatch(string path)
    {
        return Regex.IsMatch(System.IO.Path.GetFileName(path), @"^\d{2}$");
    }

    public static MonthDirectory FromPath(int year, string path)
    {
        return new MonthDirectory(year, int.Parse(System.IO.Path.GetFileName(path)), path);
    }
}
