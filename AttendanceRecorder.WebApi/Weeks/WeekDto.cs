namespace AttendanceRecorder.WebApi.Weeks;

public class WeekDto
{
    public int Year { get; set; }
    public int Number { get; set; }
    public DateDto FirstDate { get; set; } = new DateDto();
    public DateDto LastDate { get; set; } = new DateDto();
}
