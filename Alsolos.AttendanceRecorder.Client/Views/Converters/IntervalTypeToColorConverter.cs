using System;
using System.Globalization;
using System.Windows.Media;
using Alsolos.AttendanceRecorder.Client.Views.Model;
using Alsolos.Commons.Wpf.Mvvm.Converters;

namespace Alsolos.AttendanceRecorder.Client.Views.Converters
{
    public class IntervalTypeToColorConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var intervalType = (IntervalType)value;
            switch (intervalType)
            {
                case IntervalType.Inactive:
                    return new SolidColorBrush(Colors.DarkRed);
                case IntervalType.Active:
                    return new SolidColorBrush(Colors.Green);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
