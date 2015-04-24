namespace Alsolos.AttendanceRecorder.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Alsolos.AttendanceRecorder.WebApi.Model;
    using Alsolos.AttendanceRecorder.WebApiModel;

    [RoutePrefix("api/intervals")]
    public class IntervalsController : ApiController
    {
        private readonly IIntervalCollection _intervalCollection;

        public IntervalsController()
        {
            _intervalCollection = WebApiStarter.IntervalCollection;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<IInterval> GetIntervals()
        {
            return _intervalCollection.Intervals;
        }

        [Route("range/{fromString}/{toString}")]
        [HttpGet]
        public IEnumerable<IInterval> GetIntervalsInRange(string fromString, string toString)
        {
            var from = new Date(DateTime.Parse(fromString));
            var to = toString != null ? new Date(DateTime.Parse(toString)) : new Date(DateTime.Now);
            return _intervalCollection.Intervals.Where(interval => interval.Date >= from && interval.Date <= to);
        }

        [Route("dates")]
        [HttpGet]
        public IEnumerable<Date> GetDates()
        {
            return _intervalCollection.Intervals.Select(interval => interval.Date).Distinct();
        }

        [Route("remove")]
        [HttpPost]
        public bool Remove([FromBody]ReceivedInterval interval)
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
