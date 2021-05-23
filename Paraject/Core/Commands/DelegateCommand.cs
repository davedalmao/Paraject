using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Paraject.Core.Commands
{
    class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action RunCommand;

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
