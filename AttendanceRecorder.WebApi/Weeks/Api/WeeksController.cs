using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceRecorder.WebApi.Weeks.Api;

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
    public IEnumerable<YearWeekDto> GetWeeks()
    {
        return _weeksService.FindAvailableWeeks().Select(week => week.Adapt<YearWeekDto>());
    }
}
