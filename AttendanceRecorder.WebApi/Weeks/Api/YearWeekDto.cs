namespace AttendanceRecorder.WebApi.Weeks.Api;

public class YearWeekDto
{
    public int Year { get; set; }
    public int Number { get; set; }
    public DateOnly FirstDate { get; set; } = new();
    public DateOnly LastDate { get; set; } = new();
}
