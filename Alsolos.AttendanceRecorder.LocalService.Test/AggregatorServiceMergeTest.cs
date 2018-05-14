using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alsolos.AttendanceRecorder.LocalService.Test
{
    [TestClass]
    public class AggregatorServiceMergeTest
    {
        [TestMethod]
        public void Merge_TwoIntervals_MergeMatches_SingeIntervalReturned()
        {
            var intervals = new[]
            {
                Helper.CreateInterval(0, 1),
                Helper.CreateInterval(3, 4)
            };
            var merges = new[]
            {
                Helper.CreateInterval(1, 3)
            };

            var results = intervals.Merge(merges).ToList();

            results.Should().HaveCount(1);
            results[0].Start.Minute.Should().Be(0);
            results[0].End.Minute.Should().Be(4);
        }
    }
}
