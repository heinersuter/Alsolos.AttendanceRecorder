using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Alsolos.AttendanceRecorder.WebApi.Model;
using NLog;

namespace Alsolos.AttendanceRecorder.WebApi.Controllers
{
    [RoutePrefix("api/intervals")]
    public class IntervalsController : ApiController
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IIntervalCollection _intervalCollection;

        public IntervalsController()
        {
            _intervalCollection = WebApiStarter.IntervalCollection;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Interval> GetIntervals()
        {
            Logger.Trace("API: GetIntervals");
            return _intervalCollection.Intervals;
        }

        [Route("range/{fromString}/{toString}")]
        [HttpGet]
        public IEnumerable<Interval> GetIntervalsInRange(string fromString, string toString)
        {
            Logger.Trace($"API: GetIntervals from {fromString} to {toString}.");
            var from = DateTime.Parse(fromString);
            var to = toString != null ? DateTime.Parse(toString) : DateTime.Now;
            return _intervalCollection.Intervals.Where(interval => interval.Start.Date >= from.Date && interval.Start.Date <= to.Date);
        }

        [Route("dates")]
        [HttpGet]
        public IEnumerable<DateTime> GetDates()
        {
            return _intervalCollection.Intervals.Select(interval => interval.Start.Date).Distinct();
        }

        [Route("remove")]
        [HttpPost]
        public bool Remove([FromBody]Interval interval)
        {
            if (interval == null)
            {
                return false;
            }
            return _intervalCollection.Remove(interval);
        }

        [Route("merge")]
        [HttpPost]
        public bool Merge([FromBody]IntervalPair intervalPair)
        {
            return _intervalCollection.Merge(intervalPair);
        }
    }
}
