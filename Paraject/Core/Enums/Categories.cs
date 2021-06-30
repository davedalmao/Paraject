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
        Database_Design,

        [Description("Database Bug")]
        Database_Bug,

        [Description("UI Design")]
        UI_Design,

        [Description("UI Bug")]
        UI_Bug,

        [Description("UX Design")]
        UX_Design,

        [Description("UX Bug")]
        UX_Bug,

        [Description("Backend")]
        Backend,

        [Description("Backend Bug")]
        Backend_Bug
    }
}