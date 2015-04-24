namespace Alsolos.AttendanceRecorder.Client.Views
{
    using System;
    using Alsolos.AttendanceRecorder.Client.Models;
    using Alsolos.AttendanceRecorder.Client.Views.Model;
    using Alsolos.AttendanceRecorder.WebApiModel;
    using Alsolos.Commons.Wpf.Mvvm;

    public class IntervalViewModel : ViewModel
    {
        public Date Date
        {
            get { return BackingFields.GetValue<Date>(); }
            set { BackingFields.SetValue(value); }
        }

        public TimeSpan Start
        {
            get { return BackingFields.GetValue<TimeSpan>(); }
            set { BackingFields.SetValue(value, x => UpdateDuration()); }
        }

        public TimeSpan End
        {
            get { return BackingFields.GetValue<TimeSpan>(); }
            set { BackingFields.SetValue(value, x => UpdateDuration()); }
        }

        public IntervalType Type
        {
            get { return BackingFields.GetValue<IntervalType>(); }
            set { BackingFields.SetValue(value); }
        }

        public TimeSpan Duration
        {
            get { return BackingFields.GetValue<TimeSpan>(); }
            private set { BackingFields.SetValue(value); }
        }

        private TimeSpan UpdateDuration()
        {
            return Duration = End - Start;
        }

        public Interval AsInterval()
        {
            return new Interval { Date = Date, Start = Start, End = End };
        }

        public bool IsDeletePossible
        {
            get { return Start != TimeSpan.Zero && End != DayViewModel.Midnight; }
        }
    }
}
