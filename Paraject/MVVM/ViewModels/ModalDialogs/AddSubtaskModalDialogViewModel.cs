using Paraject.Core.Commands;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class AddSubtaskModalDialogViewModel : BaseViewModel
    {
        public AddSubtaskModalDialogViewModel()
        {
            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);
        }

        public ICommand CloseModalDialogCommand { get; }

        private void CloseModalDialog()
        {
            MainWindowViewModel.Overlay = false;

            foreach (Window currentModal in Application.Current.Windows)
            {
                if (currentModal.DataContext == this)
                {
                    currentModal.Close();
                }
            }
        }
    }
}
