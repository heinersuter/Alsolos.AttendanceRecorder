using System;
using Alsolos.AttendanceRecorder.WebApi.Model;

namespace Alsolos.AttendanceRecorder.LocalService.Test
{
    public static class Helper
    {
        public static DateTime CreateDateTime(int day = 1, int hour = 6, int minute = 0, int second = 0)
        {
            return new DateTime(2000, 1, day, hour, minute, second, DateTimeKind.Local);
        }

        public static Interval CreateInterval(int startMinute, int endMinute)
        {
            return new Interval
            {
                Start = CreateDateTime(minute: startMinute),
                End = CreateDateTime(minute: endMinute)
            };
        }
    }
}
