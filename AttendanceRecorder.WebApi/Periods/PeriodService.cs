using AttendanceRecorder.WebApi.FileSystem;

namespace AttendanceRecorder.WebApi.Periods;

public class PeriodService
{
    private static readonly TimeSpan Threshold = TimeSpan.FromMinutes(1);

    private readonly FileService _fileService;

    public PeriodService(FileService fileService)
    {
        _fileService = fileService;
    }

    public IEnumerable<PeriodDay> GetPeriodDays(DateOnly firstDate, DateOnly lastDate)
    {
        var dayFiles = _fileService.GetDayFiles(firstDate, lastDate);

        return dayFiles.Select(GetPeriodDay);
    }

    private static PeriodDay GetPeriodDay(DayFile dayFile)
    {
        var periods = AggregateTimesToPeriods(FileService.ReadDayFile(dayFile));

        return new PeriodDay(dayFile.Date, periods.ToList());
    }

    private static IEnumerable<Period> AggregateTimesToPeriods(IEnumerable<TimeOnly> times)
    {
        TimeOnly? currentPeriodStartTime = null;
        TimeOnly? currentPeriodEndTime = null;

        foreach (var time in times.OrderBy(t => t))
        {
            if (currentPeriodStartTime == null)
            {
                currentPeriodStartTime = time;
                currentPeriodEndTime = time;
            }

            if (currentPeriodEndTime!.Value.Add(Threshold) > time)
            {
                currentPeriodEndTime = time;
            }
            else
            {
                yield return new Period(currentPeriodStartTime.Value, currentPeriodEndTime.Value);
                currentPeriodStartTime = time;
                currentPeriodEndTime = time;
            }
        }

        if (currentPeriodStartTime != null && currentPeriodEndTime != null)
        {
            yield return new Period(currentPeriodStartTime.Value, currentPeriodEndTime.Value);
        }
    }
}
