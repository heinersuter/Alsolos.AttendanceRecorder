namespace Alsolos.AttendanceRecorder.LocalService
{
    using System;
    using System.Linq;
    using Alsolos.AttendanceRecorder.WebApi;
    using Alsolos.AttendanceRecorder.WebApi.Model;
    using Alsolos.AttendanceRecorder.WebApiModel;

    public class AttendanceRecorderService : IDisposable
    {
        private readonly IntervalAggregator _aggregator;
        private readonly WebApiStarter _webApiStarter;

        public AttendanceRecorderService()
        {
            _aggregator = new IntervalAggregator();
            _webApiStarter = new WebApiStarter(_aggregator);
            _webApiStarter.Start();
        }

        public void KeepAlive(string timeAccountName, TimeSpan updatePeriod)
        {
            AddOrUpdateInterval(timeAccountName, updatePeriod, DateTime.Now);
        }

        public void Dispose()
        {
            if (_webApiStarter != null)
            {
                _webApiStarter.Dispose();
            }
        }

        private void AddOrUpdateInterval(string timeAccountName, TimeSpan updatePeriod, DateTime currentDateTime)
        {
            var currentInterval = FindIntervallToUpdate(timeAccountName, updatePeriod, currentDateTime);

            if (currentInterval != null)
            {
                UpdateExistingIntervall(currentInterval, currentDateTime);
                _aggregator.SaveIntervals();
            }
            else
            {
                _aggregator.Add(CreateNewIntervall(timeAccountName, currentDateTime));
            }
        }

        private IInterval FindIntervallToUpdate(string timeAccountName, TimeSpan updatePeriod, DateTime currentTime)
        {
            var currentInterval =
                _aggregator.Intervals.SingleOrDefault(
                    interval => interval.TimeAccountName == timeAccountName
                        && interval.Date == new Date(currentTime.Date)
                        && currentTime.Date + interval.End + updatePeriod + updatePeriod > currentTime);
            return currentInterval;
        }

        private static Interval CreateNewIntervall(string timeAccountName, DateTime currentTime)
        {
            var currentInterval = new Interval
            {
                State = IntervalState.New,
                Date = new Date(currentTime.Date),
                Start = GetTimeOfDay(currentTime),
                End = GetTimeOfDay(currentTime),
                TimeAccountName = timeAccountName,
                LastModified = DateTime.Now,
            };
            return currentInterval;
        }

        private static void UpdateExistingIntervall(IInterval currentInterval, DateTime currentTime)
        {
            currentInterval.State = IntervalState.Dirty;
            currentInterval.End = GetTimeOfDay(currentTime);
            currentInterval.LastModified = DateTime.Now;
        }

        private static TimeSpan GetTimeOfDay(DateTime currentTime)
        {
            return new TimeSpan(currentTime.Hour, currentTime.Minute, currentTime.Second);
        }
    }
}
