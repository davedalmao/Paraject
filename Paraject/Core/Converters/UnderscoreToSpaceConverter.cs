using System;
using System.Globalization;
using System.Windows.Data;

namespace Paraject.Core.Converters
{
    public class UnderscoreToSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = System.Convert.ToString(value);

            text = text.Replace("_", " ");

            return text;
        }
    }
}
