using System.Collections.Generic;

namespace Alsolos.AttendanceRecorder.WebApi.Intervals
{
    public interface IIntervalCollection
    {
        IEnumerable<Interval> Intervals { get; }

        bool Remove(Interval interval);

        bool Merge(IntervalPair intervalPair);
    }
}
