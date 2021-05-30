using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.Views.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.Windows
{
    class LoginWindowViewModel : BaseViewModel
    {
        private readonly UserAccountRepository _userAccountRepository;
        private DelegateCommand _signUpWindowRedirectCommand;
        public event EventHandler Closed; //The Window (LoginWindow) closes itself when this event is executed

        public LoginWindowViewModel()
        {
            _userAccountRepository = new UserAccountRepository();
            CurrentUserAccount = new UserAccount();
            LoginCommand = new DelegateCommand(Login);
        }

        public DelegateCommand SignUpWindowRedirectCommand
        {
            get { return _signUpWindowRedirectCommand ??= new DelegateCommand(ShowSignupWindow); }
        }
        public UserAccount CurrentUserAccount { get; set; }
        public ICommand LoginCommand { get; set; }

        public void Login()
        {
            if (ValidateInput())
            {
                ShowMainWindow();
            }

            else
            {
                MessageBox.Show("Check your inputs: username does not exist, or incorrect password.");
            }
        }

        public bool ValidateInput()
        {
            if (!string.IsNullOrWhiteSpace(CurrentUserAccount.Username) && !string.IsNullOrWhiteSpace(CurrentUserAccount.Password))
            {
                return _userAccountRepository.AccountExistsInDatabase(CurrentUserAccount);
            }
            return false;
        }

        #region Methods
        public void ShowSignupWindow()
        {
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.Show();
            Close(); //Closes LoginWindow when SignupWindow is present
        }
        private void ShowMainWindow()
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close(); //Closes LoginWindow when MainWindow is present
        }
        private void Close() //The method that executes Closed EventHandler
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
