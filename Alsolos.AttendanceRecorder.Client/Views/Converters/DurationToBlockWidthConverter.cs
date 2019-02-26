using System;
using System.Globalization;
using Alsolos.Commons.Wpf.Mvvm.Converters;

namespace Alsolos.AttendanceRecorder.Client.Views.Converters
{
    public class DurationToBlockWidthConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var duration = (TimeSpan)value;
            return 800.0 / 24.0 * duration.TotalHours;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
