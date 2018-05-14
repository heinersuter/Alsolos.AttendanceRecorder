namespace Alsolos.AttendanceRecorder.Client.Views
{
    using System;
    using Alsolos.AttendanceRecorder.Client.Models;
    using Alsolos.AttendanceRecorder.Client.Views.Model;
    using Alsolos.Commons.Wpf.Mvvm;

    public class IntervalViewModel : ViewModel
    {
        public DateTime Date
        {
            get { return BackingFields.GetValue<DateTime>(); }
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

        public bool IsDeletePossible
        {
            get { return Start != TimeSpan.Zero && End != DayViewModel.Midnight; }
        }

        public Interval AsInterval()
        {
            return new Interval { Start = Date.Add(Start), End = Date.Add(End) };
        }

        private TimeSpan UpdateDuration()
        {
            return Duration = End - Start;
        }
    }
}
