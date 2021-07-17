using Paraject.Core.Commands;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.MessageBoxes
{
    public class OkayMessageBoxViewModel
    {
        public OkayMessageBoxViewModel(string title, string message)
        {
            Title = title;
            Message = message;

            CloseMessageBoxCommand = new DelegateCommand(CloseModalDialog);
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public ICommand CloseMessageBoxCommand { get; }

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
