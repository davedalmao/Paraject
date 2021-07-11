using System;
using System.Globalization;
using System.Windows.Data;

namespace Paraject.Core.Converters
{
    /// <summary>
    /// This is used in SubtasksView's header
    /// </summary>
    public class UnderscoreToSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"[ {value.ToString().Replace("_", " ")} ]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
