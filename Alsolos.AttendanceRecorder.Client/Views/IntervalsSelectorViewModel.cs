namespace Alsolos.AttendanceRecorder.Client.Views
{
    using System.ComponentModel;
    using Alsolos.Commons.Wpf.Mvvm;

    public class IntervalsSelectorViewModel : ViewModel
    {
        public IntervalsSelectorViewModel()
        {
            DatePeriodViewModel.PropertyChanged += OnDatePeriodViewModelPropertyChanged;
            DatePeriodViewModel.Load();
        }

        public DatePeriodViewModel DatePeriodViewModel
        {
            get { return BackingFields.GetValue(() => new DatePeriodViewModel()); }
        }

        public DaysViewModel DaysViewModel
        {
            get { return BackingFields.GetValue(() => new DaysViewModel()); }
        }

        private void OnDatePeriodViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GetPropertyName(() => DatePeriodViewModel.SelectedPeriod))
            {
                DaysViewModel.DatePeriod = DatePeriodViewModel.SelectedPeriod;
            }
        }
    }
}
