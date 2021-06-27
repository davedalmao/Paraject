using Paraject.Core.Enums.EnumBinding;
using System.ComponentModel;

namespace Paraject.Core.Enums
{
    //This reads the Enum "Description" instead of the Enum itself (when displayed in a ComboBox)
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]

    public enum Categories
    {
        [Description("Database")]
        Database,

        [Description("Database Design")]
        DatabaseDesign,

        [Description("Database Bug")]
        DatabaseBug,

        [Description("UI Design")]
        UiDesign,

        [Description("UI Bug")]
        UiBug,

        [Description("UX Design")]
        UxDesign,

        [Description("UX Bug")]
        UxBug,

        [Description("Backend")]
        Backend,

        [Description("Backend Bug")]
        BackendBug
    }
}