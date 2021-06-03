using PropertyChanged;
using System.ComponentModel;

namespace Paraject.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
