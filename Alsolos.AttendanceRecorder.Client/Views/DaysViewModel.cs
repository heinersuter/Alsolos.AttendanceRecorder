namespace Alsolos.AttendanceRecorder.Client.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Alsolos.AttendanceRecorder.Client.Models;
    using Alsolos.AttendanceRecorder.Client.Services;
    using Alsolos.AttendanceRecorder.Client.Views.Model;
    using Alsolos.Commons.Controls.Progress;

    public class DaysViewModel : BusyViewModel
    {
        private readonly IntervalService _intervalService = new IntervalService();

        public DatePeriod DatePeriod
        {
            get { return BackingFields.GetValue<DatePeriod>(); }
            set { BackingFields.SetValue(value, period => Load()); }
        }

        public TimeSpan TotalTime
        {
            get { return BackingFields.GetValue<TimeSpan>(); }
            private set { BackingFields.SetValue(value); }
        }

        public IList<DayViewModel> Days
        {
            get { return BackingFields.GetValue<IList<DayViewModel>>(); }
            private set { BackingFields.SetValue(value); }
        }

        private async void Load()
        {
            using (BusyHelper.Enter("Loading IntervalService..."))
            {
                DetachPropertyChangedEventHandler();

                var intervals = await _intervalService.GetIntervalsInRangeAsync(DatePeriod.Start, DatePeriod.End);
                await SetDayViewModels(intervals);
                ExpandToday();
                CalculateTotalTime();
            }
        }

        private async Task SetDayViewModels(IEnumerable<Interval> intervals)
        {
            await Task.Run(() =>
            {
                if (Days != null)
                {
                    foreach (var dayViewModel in Days)
                    {
                        dayViewModel.Dispose();
                    }
                }

                var dayGroupings = intervals.GroupBy(interval => interval.Date);
                Days = dayGroupings.Select(grouping =>
                {
                    var dayViewModel = new DayViewModel(grouping.Key, grouping.ToList());
                    dayViewModel.PropertyChanged += OnDayViewModelPropertyChanged;
                    return dayViewModel;
                }).OrderByDescending(dayViewModel => dayViewModel.Date).ToList();
            });
        }

        private void DetachPropertyChangedEventHandler()
        {
            if (Days != null)
            {
                foreach (var dayViewModel in Days)
                {
                    dayViewModel.PropertyChanged -= OnDayViewModelPropertyChanged;
                }
            }
        }

        private void OnDayViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == GetPropertyName(() => ((DayViewModel)sender).TotalTime))
            {
                CalculateTotalTime();
            }
        }

        private void ExpandToday()
        {
            var firstDay = Days.FirstOrDefault();
            if (firstDay != null && firstDay.Date.DateTime.Date == DateTime.Now.Date)
            {
                firstDay.IsExpanded = true;
            }
        }

        private void CalculateTotalTime()
        {
            if (Days != null)
            {
                TotalTime = Days.Aggregate(TimeSpan.Zero, (total, currentViewModel) => total + currentViewModel.TotalTime);
            }
            else
            {
                TotalTime = TimeSpan.Zero;
            }
        }
    }
}
