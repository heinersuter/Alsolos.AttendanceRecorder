namespace Alsolos.AttendanceRecorder.WebApi.Model
{
    using System.Collections.Generic;

    public interface IIntervalCollection
    {
        IEnumerable<IInterval> Intervals { get; }

        bool Remove(IInterval interval);

        bool Merge(IntervalPair intervalPair);
    }
}
