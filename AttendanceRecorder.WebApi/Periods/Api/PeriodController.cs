using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceRecorder.WebApi.Periods.Api;

[ApiController]
[Route("api/periods")]
public class PeriodController : ControllerBase
{
    private readonly PeriodService _periodService;

    public PeriodController(PeriodService periodService)
    {
        _periodService = periodService;
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<PeriodDayDto> GetPeriods([FromQuery] DateOnly firstDate, [FromQuery] DateOnly lastDate)
    {
        return _periodService.GetPeriodDays(firstDate, lastDate).Select(period => period.Adapt<PeriodDayDto>());
    }
}
