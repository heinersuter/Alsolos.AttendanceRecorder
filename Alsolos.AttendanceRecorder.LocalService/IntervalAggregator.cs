namespace Alsolos.AttendanceRecorder.LocalService
{
    using System.Collections.Generic;
    using System.Linq;
    using Alsolos.AttendanceRecorder.WebApi.Model;

    public class IntervalAggregator : IIntervalCollection
    {
        private readonly IList<Interval> _intervals;
        private readonly LocalFileSystemStore _localFileSystemStore = new LocalFileSystemStore();

        public IntervalAggregator()
        {
            _intervals = _localFileSystemStore.Load();
        }

        public IEnumerable<IInterval> Intervals
        {
            get { return _intervals.Cast<IInterval>().ToList(); }
        }

        public void Add(Interval interval)
        {
            _intervals.Add(interval);
            SaveIntervals();
        }

        public bool Remove(IInterval interval)
        {
            var intervalToRemove = _intervals.SingleOrDefault(inner => AreEqual(inner, interval));
            if (intervalToRemove != null)
            {
                var result = _intervals.Remove(intervalToRemove);
                if (result)
                {
                    SaveIntervals();
                }
                return result;
            }
            return false;
        }

        public bool Merge(IntervalPair intervalPair)
        {
            var intervalToExtend = _intervals.SingleOrDefault(inner => AreEqual(inner, intervalPair.Interval1));
            var intervalToRemove = _intervals.SingleOrDefault(inner => AreEqual(inner, intervalPair.Interval2));
            if (intervalToExtend != null &&
                intervalToRemove != null &&
                intervalToExtend.Date == intervalToRemove.Date &&
                intervalToExtend.Start < intervalToRemove.Start)
            {
                intervalToExtend.End = intervalToRemove.End;
                return Remove(intervalToRemove);
            }
            return false;
        }

        public void SaveIntervals()
        {
            _localFileSystemStore.Save(_intervals.Where(i => i.State != IntervalState.Persisted).ToList());
        }

        private bool AreEqual(IInterval localInterval, IInterval otherInterval)
        {
            return localInterval.Date == otherInterval.Date && localInterval.Start == otherInterval.Start;
        }
    }
}
