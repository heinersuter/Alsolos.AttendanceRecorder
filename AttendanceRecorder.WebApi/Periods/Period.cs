namespace AttendanceRecorder.WebApi.Periods;

public class Period
{
    public Period(TimeOnly start, TimeOnly end)
    {
        Start = start;
        End = end;
    }

    public TimeOnly Start { get; }

    public TimeOnly End { get; }
}
