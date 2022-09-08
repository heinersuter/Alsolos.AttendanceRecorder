using System.Text.RegularExpressions;

namespace AttendanceRecorder.WebApi.FileSystem;

public struct DayFile
{
    public DayFile(DateOnly date, string path)
    {
        Date = date;
        Path = path;
    }

    public DateOnly Date { get; }

    public string Path { get; }

    public static bool IsMatch(string path)
    {
        return Regex.IsMatch(System.IO.Path.GetFileName(path), @"^\d{2}\.att$");
    }

    public static DayFile FromPath(int year, int month, string path)
    {
        return new DayFile(new DateOnly(year, month, int.Parse(System.IO.Path.GetFileNameWithoutExtension(path))), path);
    }
}
