using System;
using System.Windows.Markup;

namespace Paraject.Core.Enums.EnumBinding
{
    /// <summary>
    /// This class is used for BINDING Enums (to a ComboBox)
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; set; }

        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType is not null || enumType.IsEnum)
            {
                EnumType = enumType;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}
