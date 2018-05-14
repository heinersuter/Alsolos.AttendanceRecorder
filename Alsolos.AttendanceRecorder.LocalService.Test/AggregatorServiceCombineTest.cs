using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alsolos.AttendanceRecorder.LocalService.Test
{
    [TestClass]
    public class AggregatorServiceCombineTest
    {
        [TestMethod]
        public void CombineToIntervals_SingleDay_SingleInterval_IntervalIsCorrect()
        {
            var lifeSigns = new[]
            {
                new DateTime(2000, 1, 1, 6, 0, 0, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 6, 0, 30, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 6, 1, 0, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 6, 1, 30, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 6, 2, 0, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 6, 2, 30, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 6, 3, 0, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 6, 3, 30, DateTimeKind.Local)
            };

            var intervals = lifeSigns.CombineToIntervals()
                .ToList();

            intervals.Should().HaveCount(1);
            intervals[0].Start.Should().Be(new DateTime(2000, 1, 1, 6, 0, 0));
            intervals[0].End.Should().Be(new DateTime(2000, 1, 1, 6, 3, 30));
        }

        [TestMethod]
        public void CombineToIntervals_SingleDay_TwoIntervals_IntervalIsCorrect()
        {
            var lifeSigns = new[]
            {
                new DateTime(2000, 1, 1, 6, 0, 0, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 6, 0, 30, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 7, 0, 0, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 7, 0, 30, DateTimeKind.Local),
            };

            var intervals = lifeSigns.CombineToIntervals()
                .ToList();

            intervals.Should().HaveCount(2);
            intervals[0].Start.Should().Be(new DateTime(2000, 1, 1, 6, 0, 0));
            intervals[0].End.Should().Be(new DateTime(2000, 1, 1, 6, 0, 30));
            intervals[1].Start.Should().Be(new DateTime(2000, 1, 1, 7, 0, 0));
            intervals[1].End.Should().Be(new DateTime(2000, 1, 1, 7, 0, 30));
        }

        [TestMethod]
        public void CombineToIntervals_TwoDays_SingleIntervalPerDay_IntervalsAreCorrect()
        {
            var lifeSigns = new[]
            {
                new DateTime(2000, 1, 1, 6, 0, 0, DateTimeKind.Local),
                new DateTime(2000, 1, 1, 6, 0, 30, DateTimeKind.Local),
                new DateTime(2000, 1, 2, 6, 2, 0, DateTimeKind.Local),
                new DateTime(2000, 1, 2, 6, 2, 30, DateTimeKind.Local),
            };

            var intervals = lifeSigns.CombineToIntervals()
                .ToList();

            intervals.Should().HaveCount(2);
            intervals[0].Start.Should().Be(new DateTime(2000, 1, 1, 6, 0, 0));
            intervals[0].End.Should().Be(new DateTime(2000, 1, 1, 6, 0, 30));
            intervals[1].Start.Should().Be(new DateTime(2000, 1, 2, 6, 2, 0));
            intervals[1].End.Should().Be(new DateTime(2000, 1, 2, 6, 2, 30));
        }
    }
}
