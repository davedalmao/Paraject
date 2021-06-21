using System;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Paraject.Core.Enums
{
    public class CamelCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string enumString = value.ToString();
            string camelCaseString = Regex.Replace(enumString, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1")
;
            return camelCaseString;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
