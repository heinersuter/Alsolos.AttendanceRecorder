namespace AttendanceRecorder.WebApi.Periods;

public class PeriodDay
{
    public PeriodDay(DateOnly date, IReadOnlyCollection<Period> periods)
    {
        Date = date;
        Periods = periods;
    }

    public DateOnly Date { get; }

    public IReadOnlyCollection<Period> Periods { get; }
}
