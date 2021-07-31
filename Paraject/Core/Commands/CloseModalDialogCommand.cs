using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs.MessageBoxes;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.Core.Commands
{
    public class CloseModalDialogCommand : ICommand
    {

        public bool CanExecute(object parameter)
        {
            return parameter is Window; //we can only close Windows
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                ((Window)parameter).Close();
                HideMainWindowOverlay((Window)parameter);
            }
        }

        private static void HideMainWindowOverlay(Window window)
        {
            //If the current "interactable" window is a custom "MessageBox", do not modify the value of the Overlay in the MainWindow
            if (window is DialogWindow) { return; }

            if (MainWindowViewModel.Overlay)
            {
                MainWindowViewModel.Overlay = false;
            }
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public static readonly ICommand Instance = new CloseModalDialogCommand();
    }
}
