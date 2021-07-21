using System;
using System.ComponentModel;
using System.Reflection;

namespace Paraject.Core.Converters
{
    public static class EnumToDescriptionConverter
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                    {
                        return attribute.Description;
                    }
                }
            }
            return null;
        }
    }
}
