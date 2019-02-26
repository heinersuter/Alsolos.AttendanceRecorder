using System;
using System.Collections.Generic;
using System.Linq;
using Alsolos.AttendanceRecorder.WebApi.Intervals;
using NLog;

namespace Alsolos.AttendanceRecorder.LocalService
{
    public static partial class AggregatorService
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static readonly TimeSpan Threshold = TimeSpan.FromMinutes(1);

        public static IEnumerable<Interval> CombineToIntervals(this IEnumerable<DateTime> lifeSigns)
        {
            Interval currentInterval = null;
            foreach (var lifeSign in lifeSigns.OrderBy(dateTime => dateTime))
            {
                if (currentInterval == null)
                {
                    currentInterval = new Interval { Start = lifeSign, End = lifeSign };
                    Logger.Trace($"Combine: New interval created starting with {lifeSign:s}");
                }

                if (currentInterval.Start.Date == lifeSign.Date && currentInterval.End.Add(Threshold) > lifeSign)
                {
                    currentInterval.End = lifeSign;
                }
                else
                {
                    Logger.Trace($"Combine: Interval completed at {lifeSign:s}");
                    yield return currentInterval;
                    currentInterval = new Interval { Start = lifeSign, End = lifeSign };
                    Logger.Trace($"Combine: New interval created starting with {lifeSign:s}");
                }
            }

            yield return currentInterval;
        }

        public static IEnumerable<Interval> Remove(this IEnumerable<Interval> intervals, IEnumerable<Interval> removals)
        {
            return intervals.Except(removals, new IntervalComparer());
        }

        public static IEnumerable<Interval> Merge(this IEnumerable<Interval> intervals, IEnumerable<Interval> merges)
        {
            var newIntervals = intervals.ToList();

            foreach (var merge in merges)
            {
                var intervalsToMerge = newIntervals.Where(interval => interval.End >= merge.Start && interval.Start <= merge.End)
                    .OrderBy(interval => interval.Start).ToList();
                if (intervalsToMerge.Any())
                {
                    foreach (var intervalToRemove in intervalsToMerge)
                    {
                        newIntervals.Remove(intervalToRemove);
                        Logger.Trace($"Merge: Intervall removed: {intervalToRemove.Start:s} - {intervalToRemove.End:s}");
                    }

                    var newInterval = new Interval { Start = intervalsToMerge.First().Start, End = intervalsToMerge.Last().End };
                    newIntervals.Add(newInterval);
                    Logger.Trace($"Merge: New interval added: {newInterval.Start:s} - {newInterval.End:s}");
                }
            }

            return newIntervals;
        }
    }
}
