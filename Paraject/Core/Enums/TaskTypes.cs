using Paraject.Core.Enums.EnumBinding;
using System.ComponentModel;

namespace Paraject.Core.Enums
{
    //This reads the Enum "Description" instead of the Enum itself (when displayed in a ComboBox)
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum TaskTypes
    {
        [Description("Finish Line")]
        Finish_Line,

        [Description("Extra Feature")]
        Extra_Feature
    }
}
