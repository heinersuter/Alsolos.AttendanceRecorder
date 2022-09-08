using AttendanceRecorder.WebApi.FileSystem;

namespace AttendanceRecorder.WebApi.Weeks;

public class WeeksService
{
    private readonly FileService _fileService;

    public WeeksService(FileService fileService)
    {
        _fileService = fileService;
    }

    public IEnumerable<YearWeek> FindAvailableWeeks()
    {
        var dates = _fileService.FindAvailableDates().ToList();
        var weeks = dates
            .Select(YearWeek.FromDate)
            .Distinct();

        return weeks;
    }
}
