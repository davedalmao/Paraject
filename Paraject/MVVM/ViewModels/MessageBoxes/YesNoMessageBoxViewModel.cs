using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Services.DialogService;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.MessageBoxes
{
    public class YesNoMessageBoxViewModel : DialogBaseViewModel<DialogResults>
    {
        public YesNoMessageBoxViewModel(string title, string message, string iconSource) : base(message, title, iconSource)
        {
            Title = title;
            Message = message;
            IconSource = iconSource;

            YesCommand = new RelayCommand<IDialogWindow>(Yes);
            NoCommand = new RelayCommand<IDialogWindow>(No);
        }

        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }


        private void Yes(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.Yes);
        }
        private void No(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.No);
        }
    }
}
