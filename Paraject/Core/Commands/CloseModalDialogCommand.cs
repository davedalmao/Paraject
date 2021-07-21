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
            }
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public static readonly ICommand Instance = new CloseModalDialogCommand();
    }
}
