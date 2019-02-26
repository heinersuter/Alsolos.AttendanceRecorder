using System.Diagnostics.CodeAnalysis;

namespace Alsolos.AttendanceRecorder.WebApi.Intervals
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Setter is used by deserialization.")]
    public class IntervalPair
    {
        public Interval Interval1 { get; set; }

        public Interval Interval2 { get; set; }
    }
}
