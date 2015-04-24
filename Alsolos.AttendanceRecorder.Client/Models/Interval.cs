namespace Alsolos.AttendanceRecorder.Client.Models
{
    using System;
    using Alsolos.AttendanceRecorder.WebApiModel;

    public class Interval
    {
        public Date Date { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}
