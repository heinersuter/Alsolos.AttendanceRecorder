using Microsoft.Extensions.Options;

namespace AttendanceRecorder.WebApi.FileSystem;

public class FileService
{
    private readonly FileSystemOptions _fileSystemOptions;

    public FileService(IOptions<FileSystemOptions> fileSystemOptions)
    {
        _fileSystemOptions = fileSystemOptions.Value;
    }

    public IEnumerable<DateOnly> FindAvailableDates()
    {
        var yearDirectories = GetYearDirectories();

        foreach (var yearDirectory in yearDirectories)
        {
            var monthDirectories = GetMonthDirectories(yearDirectory);

            foreach (var monthDirectory in monthDirectories)
            {
                var dayFiles = GetDayFiles(monthDirectory);

                foreach (var dayFile in dayFiles)
                {
                    yield return dayFile.Date;
                }
            }
        }
    }

    public IEnumerable<DayFile> GetDayFiles(DateOnly firstDate, DateOnly lastDate)
    {
        var yearDirectories = GetYearDirectories(firstDate.Year, lastDate.Year);

        foreach (var yearDirectory in yearDirectories)
        {
            var monthDirectories = GetMonthDirectories(
                yearDirectory,
                yearDirectory.Year == firstDate.Year ? firstDate.Month : 1,
                yearDirectory.Year == lastDate.Year ? lastDate.Month : 12);

            foreach (var monthDirectory in monthDirectories)
            {
                var dayFiles = GetDayFiles(
                    monthDirectory,
                    monthDirectory.Year == firstDate.Year && monthDirectory.Month == firstDate.Month ? firstDate.Day : 1,
                    monthDirectory.Year == lastDate.Year && monthDirectory.Month == lastDate.Month ? lastDate.Day : 31);

                foreach (var dayFile in dayFiles)
                {
                    yield return dayFile;
                }
            }
        }
    }

    public static IEnumerable<TimeOnly> ReadDayFile(DayFile dayFile)
    {
        return File.ReadAllLines(dayFile.Path)
            .Select(line => TimeOnly.ParseExact(line, "HH:mm:ss"));
    }

    private IEnumerable<YearDirectory> GetYearDirectories()
    {
        return Directory.EnumerateDirectories(_fileSystemOptions.LocalDirectory)
            .Where(YearDirectory.IsMatch)
            .Select(YearDirectory.FromPath);
    }

    private IEnumerable<YearDirectory> GetYearDirectories(int firstYear, int lastYear)
    {
        return Directory.EnumerateDirectories(_fileSystemOptions.LocalDirectory)
            .Where(YearDirectory.IsMatch)
            .Select(YearDirectory.FromPath)
            .Where(yearDirectory => yearDirectory.Year >= firstYear && yearDirectory.Year <= lastYear);
    }

    private static IEnumerable<MonthDirectory> GetMonthDirectories(YearDirectory yearDirectory)
    {
        return Directory.EnumerateDirectories(yearDirectory.Path)
            .Where(MonthDirectory.IsMatch)
            .Select(path => MonthDirectory.FromPath(yearDirectory.Year, path));
    }

    private static IEnumerable<MonthDirectory> GetMonthDirectories(YearDirectory yearDirectory, int firstMonth, int lastMonth)
    {
        return Directory.EnumerateDirectories(yearDirectory.Path)
            .Where(MonthDirectory.IsMatch)
            .Select(path => MonthDirectory.FromPath(yearDirectory.Year, path))
            .Where(monthDirectory => monthDirectory.Month >= firstMonth && monthDirectory.Month <= lastMonth);
    }

    private static IEnumerable<DayFile> GetDayFiles(MonthDirectory monthDirectory)
    {
        return Directory.EnumerateFiles(monthDirectory.Path)
            .Where(DayFile.IsMatch)
            .Select(path => DayFile.FromPath(monthDirectory.Year, monthDirectory.Month, path));
    }

    private static IEnumerable<DayFile> GetDayFiles(MonthDirectory monthDirectory, int firstDay, int lastDay)
    {
        return Directory.EnumerateFiles(monthDirectory.Path)
            .Where(DayFile.IsMatch)
            .Select(path => DayFile.FromPath(monthDirectory.Year, monthDirectory.Month, path))
            .Where(dayFile => dayFile.Date.Day >= firstDay && dayFile.Date.Day <= lastDay);
    }
}
