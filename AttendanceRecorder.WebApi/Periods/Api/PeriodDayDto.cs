namespace AttendanceRecorder.WebApi.Periods.Api;

public class PeriodDayDto
{
    public DateOnly Date { get; set; }
    
    public IEnumerable<Period> Periods { get; set; }
}
