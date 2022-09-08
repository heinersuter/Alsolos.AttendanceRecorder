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
    [Route("firstDate/lastDate")]
    public IEnumerable<PeriodDayDto> GetPeriods([FromRoute] DateOnly firstDate, [FromRoute] DateOnly lastDate)
    {
        return _periodService.GetPeriodDays(firstDate, lastDate).Select(period => period.Adapt<PeriodDayDto>());
    }
}
