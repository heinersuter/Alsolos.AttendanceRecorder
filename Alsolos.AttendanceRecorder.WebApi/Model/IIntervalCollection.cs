namespace Alsolos.AttendanceRecorder.WebApi.Model
{
    using System.Collections.Generic;

    public interface IIntervalCollection
    {
        IEnumerable<Interval> Intervals { get; }

        bool Remove(Interval interval);

        bool Merge(IntervalPair intervalPair);
    }
}
