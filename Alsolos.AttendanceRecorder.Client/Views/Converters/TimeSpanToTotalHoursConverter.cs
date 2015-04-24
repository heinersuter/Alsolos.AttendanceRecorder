namespace Alsolos.AttendanceRecorder.Client.Views.Converters
{
    using System;
    using System.Globalization;
    using Alsolos.Commons.Mvvm.Converters;

    public class TimeSpanToTotalHoursConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan)value;

            return string.Format("{0:D}:{1:D2}:{2:D2}", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
