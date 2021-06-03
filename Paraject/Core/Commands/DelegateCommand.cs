using System;
using System.Windows.Input;

namespace Paraject.Core.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action RunCommand;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action runCommand)
        {
            RunCommand = runCommand;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            RunCommand();
        }
    }
}
