using System;
using Alsolos.AttendanceRecorder.Client.Models;
using Alsolos.Commons.Wpf.Mvvm;

namespace Alsolos.AttendanceRecorder.Client.Views
{
    public class ChangeStartWindowViewModel : ViewModel
    {
        private DateTime _date;
        public ChangeStartWindowViewModel(IntervalViewModel previous)
        {
            _date = previous.Date;

            Min = previous.Start;
            Max = previous.End;

            Start = Max;
        }

        public TimeSpan Min { get; }

        public int MinMinutes => (int)Min.TotalMinutes;

        public TimeSpan Max { get; }

        public int MaxMinutes => (int)Max.TotalMinutes;

        public TimeSpan Start
        {
            get => BackingFields.GetValue<TimeSpan>();
            set => BackingFields.SetValue(value, span => RaisePropertyChanged(nameof(StartMinutes)));
        }

        public int StartMinutes
        {
            get => (int)Start.TotalMinutes;
            set => Start = TimeSpan.FromMinutes(value);
        }

        public Interval GetNewInterval()
        {
            return new Interval
            {
                Start = _date.Add(Start),
                End = _date.Add(Max).AddSeconds(1)
            };
        }
    }
}
