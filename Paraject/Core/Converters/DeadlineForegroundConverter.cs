using System;
using System.Globalization;
using System.Windows.Data;

namespace Paraject.Core.Converters
{
    /// <summary>
    /// If A Project, Task, and Subtask is past the deadline, then change the Deadline foreground, and icon to Red.
    /// </summary>
    public class DeadlineForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not DateTime) { return false; }

            return DateTime.Now.Date > (DateTime)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
