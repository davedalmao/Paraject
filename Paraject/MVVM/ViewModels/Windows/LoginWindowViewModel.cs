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

        #region Properties
        public DelegateCommand SignUpWindowRedirectCommand
        {
            get { return _signUpWindowRedirectCommand ??= new DelegateCommand(ShowSignupWindow); }
        }
        public UserAccount CurrentUserAccount { get; set; }
        public ICommand LoginCommand { get; }
        #endregion 

        #region Login Methods
        private void Login()
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
        private bool ValidateInput()
        {
            if (!string.IsNullOrWhiteSpace(CurrentUserAccount.Username) && !string.IsNullOrWhiteSpace(CurrentUserAccount.Password))
            {
                return _userAccountRepository.AccountExistsInDatabase(CurrentUserAccount);
            }
            return false;
        }
        #endregion

        #region Specific Window Methods
        private void ShowSignupWindow()
        {
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.Show();
            Close(); //Closes LoginWindow when SignupWindow is present
        }
        private void ShowMainWindow()
        {
            UserAccount userToLogin = _userAccountRepository.Get(CurrentUserAccount.Username);
            MainWindow mainWindow = new(userToLogin);
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
