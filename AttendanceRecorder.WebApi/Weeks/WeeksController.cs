using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceRecorder.WebApi.Weeks;

[ApiController]
[Route("api/weeks")]
public class WeeksController : ControllerBase
{
    private readonly WeeksService _weeksService;

    public WeeksController(WeeksService weeksService)
    {
        _weeksService = weeksService;
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<WeekDto> GetWeeks()
    {
        TypeAdapterConfig<Date, DateDto>.NewConfig();
        return _weeksService.FindAvailableWeeks().Select(week => week.Adapt<WeekDto>());
    }
}
