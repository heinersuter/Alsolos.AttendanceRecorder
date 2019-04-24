using System.Collections.Generic;
using Alsolos.AttendanceRecorder.WebApi.Intervals;

namespace Alsolos.AttendanceRecorder.LocalService
{
    public class IntervalComparer : IEqualityComparer<Interval>
    {
        public bool Equals(Interval x, Interval y)
        {
            return x?.Start == y?.Start && x?.End == y?.End;
        }

        public int GetHashCode(Interval obj)
        {
            unchecked
            {
                var hashCode = obj.Start.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.End.GetHashCode();
                return hashCode;
            }
        }
    }
}
