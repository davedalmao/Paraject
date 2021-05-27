using Paraject.MVVM.ViewModels.Windows;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Paraject.Core.Commands
{
    public class CreateAccountCommand : ICommand
    {
        private readonly SignupWindowViewModel _viewModel;
        public CreateAccountCommand(SignupWindowViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.PropertyChanged += SignupWindowViewModel_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.CurrentUserAccount.Username) && !string.IsNullOrEmpty(_viewModel.CurrentUserAccount.Password);
        }

        public void Execute(object parameter)
        {
            MessageBox.Show($"Username: {_viewModel.CurrentUserAccount.Username}\nPassword: {_viewModel.CurrentUserAccount.Password}", "Info",
               MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void SignupWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
