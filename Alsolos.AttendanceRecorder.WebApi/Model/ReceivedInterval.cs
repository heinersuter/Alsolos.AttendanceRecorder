namespace Alsolos.AttendanceRecorder.WebApi.Model
{
    using System;
    using Alsolos.AttendanceRecorder.WebApiModel;

    public class ReceivedInterval : IInterval
    {
        public int Id { get; set; }

        public IntervalState State { get; set; }

        public Date Date { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public DateTime LastModified { get; set; }

        public string TimeAccountName { get; set; }
    }
}
