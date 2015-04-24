namespace Alsolos.AttendanceRecorder.WebApi.Model
{
    using System;
    using Alsolos.AttendanceRecorder.WebApiModel;

    public interface IInterval
    {
        int Id { get; set; }

        IntervalState State { get; set; }

        Date Date { get; set; }

        TimeSpan Start { get; set; }

        TimeSpan End { get; set; }

        DateTime LastModified { get; set; }

        string TimeAccountName { get; set; }
    }
}
