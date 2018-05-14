namespace Alsolos.AttendanceRecorder.Client.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using Alsolos.AttendanceRecorder.Client.Models;
    using Alsolos.AttendanceRecorder.Client.Services;
    using Alsolos.AttendanceRecorder.Client.Views.Model;
    using Alsolos.Commons.Wpf.Controls.Progress;
    using Alsolos.Commons.Wpf.Mvvm;

    public class DayViewModel : BusyViewModel, IDisposable
    {
        public static readonly TimeSpan Midnight = new TimeSpan(23, 59, 59);
        private readonly TimeSpan _refreshTimerInterval = TimeSpan.FromSeconds(10);
        private readonly IntervalService _intervalService = new IntervalService();
        private readonly Timer _timer;

        public DayViewModel(DateTime date, IList<Interval> modelIntervals)
        {
            Date = date;
            Init(modelIntervals.Where(interval => interval.Start.Date == Date).OrderBy(interval => interval.Start).ToList());

            if (Date == DateTime.Now.Date)
            {
                _timer = new Timer(_refreshTimerInterval.TotalMilliseconds);
                _timer.Elapsed += (sender, args) =>
                {
                    ReloadIntervals();
                };
                _timer.Start();
            }
        }

        public DateTime Date
        {
            get { return BackingFields.GetValue<DateTime>(); }
            private set { BackingFields.SetValue(value); }
        }

        public IList<IntervalViewModel> Intervals
        {
            get { return BackingFields.GetValue<IList<IntervalViewModel>>(); }
            private set { BackingFields.SetValue(value); }
        }

        public TimeSpan TotalTime
        {
            get
            {
                return Intervals
                    .Where(viewModel => viewModel.Type == IntervalType.Active)
                    .Aggregate(TimeSpan.Zero, (total, currentViewModel) => total + currentViewModel.Duration);
            }
        }

        public bool IsExpanded
        {
            get { return BackingFields.GetValue<bool>(); }
            set { BackingFields.SetValue(value); }
        }

        public DelegateCommand<IntervalViewModel> DeleteCommand
        {
            get { return BackingFields.GetCommand<IntervalViewModel>(Delete); }
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }
        }

        private async void Delete(IntervalViewModel interval)
        {
            using (BusyHelper.Enter("Removing intervals..."))
            {
                if (interval.Type == IntervalType.Active)
                {
                    await _intervalService.RemoveIntervalAsync(interval.AsInterval());
                    ReloadIntervals();
                }
                else if (interval.Type == IntervalType.Inactive)
                {
                    var currentIndex = Intervals.IndexOf(interval);
                    var previous = Intervals[currentIndex - 1];
                    var next = Intervals[currentIndex + 1];
                    await _intervalService.MergeIntervalsAsync(previous.AsInterval(), next.AsInterval());
                    ReloadIntervals();
                }
            }
        }

        private async void ReloadIntervals()
        {
            using (BusyHelper.Enter("Loading intervals..."))
            {
                var intervals = await _intervalService.GetIntervalsInRangeAsync(Date, Date);
                Init(intervals.ToList());
            }
        }

        private void Init(IList<Interval> modelIntervals)
        {
            if (modelIntervals == null || !modelIntervals.Any())
            {
                Intervals = new List<IntervalViewModel>();
                return;
            }

            var lastTime = TimeSpan.Zero;
            var intervals = new List<IntervalViewModel>();
            foreach (var interval in modelIntervals)
            {
                if (interval.Start.TimeOfDay > lastTime)
                {
                    // Add inactive intervall
                    intervals.Add(new IntervalViewModel { Date = Date, Start = lastTime, End = interval.Start.TimeOfDay - TimeSpan.FromSeconds(1), Type = IntervalType.Inactive });
                }

                // Add active intervall
                intervals.Add(new IntervalViewModel { Date = Date, Start = interval.Start.TimeOfDay, End = interval.End.TimeOfDay, Type = IntervalType.Active });
                lastTime = interval.End.TimeOfDay;
            }
            if (lastTime < Midnight)
            {
                // Add last inactive interval to midnight
                intervals.Add(new IntervalViewModel { Date = Date, Start = lastTime + TimeSpan.FromSeconds(1), End = Midnight, Type = IntervalType.Inactive });
            }
            Intervals = new List<IntervalViewModel>(intervals);
            RaisePropertyChanged(() => TotalTime);
        }
    }
}
