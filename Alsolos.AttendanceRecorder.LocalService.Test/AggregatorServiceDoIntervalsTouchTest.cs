using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alsolos.AttendanceRecorder.LocalService.Test
{
    [TestClass]
    public class AggregatorServiceDoIntervalsTouchTest
    {
        // 1.:   --
        // 2.: ------
        [TestMethod]
        public void DoIntervalsTouch_FirstInsideSecond_True()
        {
            var first = Helper.CreateInterval(3, 4);
            var second = Helper.CreateInterval(1, 6);

            var result = first.DoIntervalsTouch(second);

            result.Should().BeTrue();
        }

        // 1.: ------
        // 2.:   --
        [TestMethod]
        public void DoIntervalsTouch_SecondInsideFirst_True()
        {
            var first = Helper.CreateInterval(1, 6);
            var second = Helper.CreateInterval(3, 4);

            var result = first.DoIntervalsTouch(second);

            result.Should().BeTrue();
        }

        // 1.: ----
        // 2.:   ----
        [TestMethod]
        public void DoIntervalsTouch_FirstOverlapsSecondAtStart_True()
        {
            var first = Helper.CreateInterval(1, 4);
            var second = Helper.CreateInterval(3, 6);

            var result = first.DoIntervalsTouch(second);

            result.Should().BeTrue();
        }

        // 1.:   ----
        // 2.: ----
        [TestMethod]
        public void DoIntervalsTouch_FirstOverlapsSecondAtEnd_True()
        {
            var first = Helper.CreateInterval(3, 6);
            var second = Helper.CreateInterval(1, 4);

            var result = first.DoIntervalsTouch(second);

            result.Should().BeTrue();
        }

        // 1.: ----
        // 2.:     ----
        [TestMethod]
        public void DoIntervalsTouch_FirstTouchesSecondAtStart_True()
        {
            var first = Helper.CreateInterval(1, 4);
            var second = Helper.CreateInterval(4, 7);

            var result = first.DoIntervalsTouch(second);

            result.Should().BeTrue();
        }

        // 1.:     ----
        // 2.: ----
        [TestMethod]
        public void DoIntervalsTouch_FirstTouchesSecondAtEnd_True()
        {
            var first = Helper.CreateInterval(4, 7);
            var second = Helper.CreateInterval(1, 4);

            var result = first.DoIntervalsTouch(second);

            result.Should().BeTrue();
        }

        // 1.: ----
        // 2.:        ----
        [TestMethod]
        public void DoIntervalsTouch_FirstIsWayBeforeSecond_False()
        {
            var first = Helper.CreateInterval(1, 4);
            var second = Helper.CreateInterval(7, 10);

            var result = first.DoIntervalsTouch(second);

            result.Should().BeFalse();
        }

        // 1.:       ----
        // 2.: ----
        [TestMethod]
        public void DoIntervalsTouch_FirstIsWayAfterSecond_False()
        {
            var first = Helper.CreateInterval(5, 8);
            var second = Helper.CreateInterval(1, 4);

            var result = first.DoIntervalsTouch(second);

            result.Should().BeFalse();
        }
    }
}
