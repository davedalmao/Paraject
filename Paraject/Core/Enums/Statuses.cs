using Paraject.Core.Enums.EnumBinding;
using System.ComponentModel;

//If items here are changed, modify ProjectRepository.cs, and ProjectStatusTheme.xaml
namespace Paraject.Core.Enums
{
    //This reads the Enum "Description" instead of the Enum itself (when displayed in a ComboBox)
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]

    public enum Statuses
    {
        [Description("In Progress")]
        InProgress,

        [Description("Open")]
        Open
    }
}
