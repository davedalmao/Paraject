using System;
using System.Collections.Generic;
using System.Linq;

namespace Paraject.Core.Enums
{
    public static class EnumExtensions
    {
        public static List<string> GetFriendlyNames(this Enum enm)
        {
            List<string> result = new();
            result.AddRange(Enum.GetNames(enm.GetType()).Select(s => s.ToFriendlyName()));
            return result;
        }

        public static string GetFriendlyName(this Enum enm)
        {
            return Enum.GetName(enm.GetType(), enm).ToFriendlyName();
        }

        private static string ToFriendlyName(this string orig)
        {
            return orig.Replace("_", " ");
        }
    }
}
