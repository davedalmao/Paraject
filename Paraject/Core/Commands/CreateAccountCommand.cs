using Paraject.MVVM.ViewModels.Windows;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Paraject.Core.Commands
{
    public class CreateAccountCommand : ICommand
    {
        private readonly SignupWindowViewModel _viewModel;
        private readonly Action RunCommand;

        public CreateAccountCommand(SignupWindowViewModel viewModel, Action runCommand)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += SignupWindowViewModel_PropertyChanged;
            RunCommand = runCommand;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.CurrentUserAccount.Username) && !string.IsNullOrEmpty(_viewModel.CurrentUserAccount.Password);
        }

        public void Execute(object parameter)
        {
            RunCommand();
        }
        private void SignupWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
