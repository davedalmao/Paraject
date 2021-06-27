using System.Text.RegularExpressions;

namespace Paraject.Core.Converters
{
    public class CamelCaseConverter
    {
        public static string CamelCaseWithSpaces(string value)
        {
            string camelCaseString = Regex.Replace(value, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1")
;
            return camelCaseString;
        }
    }
}
