using System.Text.RegularExpressions;

namespace AttendanceRecorder.WebApi.FileSystem;

public struct YearDirectory
{
    public YearDirectory(int year, string path)
    {
        Year = year;
        Path = path;
    }

    public int Year { get; }

    public string Path { get; }

    public static bool IsMatch(string path)
    {
        return Regex.IsMatch(System.IO.Path.GetFileName(path), @"^\d{4}$");
    }

    public static YearDirectory FromPath(string path)
    {
        return new YearDirectory(int.Parse(System.IO.Path.GetFileName(path)), path);
    }
}
