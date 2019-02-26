using System;
using System.Collections.Generic;
using System.Linq;
using Alsolos.AttendanceRecorder.WebApi.Intervals;
using NLog;

namespace Alsolos.AttendanceRecorder.LocalService
{
    public class IntervalCollection : IIntervalCollection
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly LocalFileSystemStore _fileSystemStore;

        public IntervalCollection(LocalFileSystemStore fileSystemStore)
        {
            _fileSystemStore = fileSystemStore;
        }

        public IEnumerable<Interval> Intervals => ReadAllIntervals();

        public bool Remove(Interval interval)
        {
            _fileSystemStore.SaveRemoval(interval);
            return true;
        }

        public bool Merge(IntervalPair intervalPair)
        {
            if (intervalPair.Interval1.Start.Date != intervalPair.Interval2.Start.Date)
            {
                throw new InvalidOperationException("Intervals must have same date to be merged.");
            }

            _fileSystemStore.SaveMerge(intervalPair);
            return true;
        }

        private IEnumerable<Interval> ReadAllIntervals()
        {
            var lifeSigns = _fileSystemStore.LoadAllLifeSigns().ToList();
            var removals = _fileSystemStore.LoadAllRemovals().ToList();
            var merges = _fileSystemStore.LoadAllMerges().ToList();

            Logger.Trace($"IntervalCollection: LifeSignes: {lifeSigns.Count}, Removals: {removals.Count}, Merges: {merges.Count}");

            var intervals = lifeSigns.CombineToIntervals().ToList();
            Logger.Trace($"IntervalCollection: Intervals: {intervals.Count}");
            intervals = intervals.Remove(removals).ToList();
            Logger.Trace($"IntervalCollection: Intervals after removals: {intervals.Count}");
            intervals = intervals.Merge(merges).ToList();
            Logger.Trace($"IntervalCollection: Intervals after merge: {intervals.Count}");

            return intervals.OrderBy(interval => interval.Start);
        }
    }
}
