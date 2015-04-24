namespace Alsolos.AttendanceRecorder.Client.Views
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Alsolos.AttendanceRecorder.Client.Services;
    using Alsolos.AttendanceRecorder.Client.Views.Model;
    using Alsolos.AttendanceRecorder.WebApiModel;
    using Alsolos.Commons.Controls.Progress;

    public class DatePeriodViewModel : BusyViewModel
    {
        public IList<DatePeriod> Years
        {
            get { return BackingFields.GetValue<IList<DatePeriod>>(); }
            private set { BackingFields.SetValue(value); }
        }

        public IList<DatePeriod> Months
        {
            get { return BackingFields.GetValue<IList<DatePeriod>>(); }
            private set { BackingFields.SetValue(value); }
        }

        public IList<DatePeriod> Weeks
        {
            get { return BackingFields.GetValue<IList<DatePeriod>>(); }
            private set { BackingFields.SetValue(value); }
        }

        public int SelectedYearIndex
        {
            get { return BackingFields.GetValue(() => -1); }
            set { BackingFields.SetValue(value, x => UpdateSelection(DatePeriodType.Year)); }
        }

        public int SelectedMonthIndex
        {
            get { return BackingFields.GetValue(() => -1); }
            set { BackingFields.SetValue(value, x => UpdateSelection(DatePeriodType.Month)); }
        }

        public int SelectedWeekIndex
        {
            get { return BackingFields.GetValue(() => -1); }
            set { BackingFields.SetValue(value, x => UpdateSelection(DatePeriodType.Week)); }
        }

        public DatePeriod SelectedPeriod
        {
            get { return BackingFields.GetValue<DatePeriod>(); }
            private set { BackingFields.SetValue(value); }
        }

        public async void Load()
        {
            using (BusyHelper.Enter("Loading dates..."))
            {
                var intervalService = new IntervalService();
                var dates = await intervalService.GetDatesAsync();
                if (dates != null)
                {
                    Init(dates.ToList());
                }
            }
        }

        public void Init(IList<Date> dates)
        {
            InitYears(dates);
            InitMonths(dates);
            InitWeeks(dates);
            SelectedWeekIndex = Weeks.Count > 0 ? 0 : -1;
        }

        private void InitYears(IEnumerable<Date> dates)
        {
            var groupings = dates.GroupBy(date => date.Year)
                .OrderBy(grouping => grouping.Key);
            Years = groupings.Select(grouping => new DatePeriod(
                DatePeriodType.Year,
                grouping.Key.ToString(CultureInfo.InvariantCulture),
                new Date(grouping.Key, 1, 1),
                new Date(grouping.Key, 12, 31))).OrderByDescending(period => period.Start).ToList();
        }

        private void InitMonths(IEnumerable<Date> dates)
        {
            var groupings = dates.GroupBy(date => new YearMonth(date.Year, date.Month))
                .OrderBy(grouping => grouping.Key.Year)
                .ThenBy(grouping => grouping.Key.Month);
            Months = groupings.Select(grouping => grouping.Key.ToDatePeriod()).OrderByDescending(period => period.Start).ToList();
        }

        private void InitWeeks(IEnumerable<Date> dates)
        {
            var groupings = dates.GroupBy(date => new YearWeek(date.DateTime))
                .OrderBy(grouping => grouping.Key.Year)
                .ThenBy(grouping => grouping.Key.Week);
            Weeks = groupings.Select(grouping => grouping.Key.ToDatePeriod()).OrderByDescending(period => period.Start).ToList();
        }

        private void UpdateSelection(DatePeriodType type)
        {
            switch (type)
            {
                case DatePeriodType.Year:
                    if (SelectedYearIndex >= 0)
                    {
                        SelectedMonthIndex = -1;
                        SelectedWeekIndex = -1;
                        SelectedPeriod = Years[SelectedYearIndex];
                    }
                    break;
                case DatePeriodType.Month:
                    if (SelectedMonthIndex >= 0)
                    {
                        SelectedYearIndex = -1;
                        SelectedWeekIndex = -1;
                        SelectedPeriod = Months[SelectedMonthIndex];
                    }
                    break;
                case DatePeriodType.Week:
                    if (SelectedWeekIndex >= 0)
                    {
                        SelectedYearIndex = -1;
                        SelectedMonthIndex = -1;
                        SelectedPeriod = Weeks[SelectedWeekIndex];
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}
