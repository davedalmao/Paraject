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
                //Login user and redirect to MainWindow
            }

            else
            {
                MessageBox.Show("Check your inputs: username does not exist, or incorrect password.");
            }
            MessageBox.Show($"Username: {CurrentUserAccount.Username} Password: {CurrentUserAccount.Password}");
        }

        public bool ValidateInput()
        {
            if (!string.IsNullOrWhiteSpace(CurrentUserAccount.Username) && !string.IsNullOrWhiteSpace(CurrentUserAccount.Password))
            {
                return AccountExistsInDatabase();
            }

            else
            {
                MessageBox.Show("Username or Password shouldn't be blank");
                return false;
            }
        }

        public bool AccountExistsInDatabase()
        {
            //Check if username and password match, and exist to database
            return false;
        }
        #region Methods
        public void ShowSignupWindow()
        {
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.Show();
            Close(); //Closes LoginWindow when SignupWindow is present
        }
        private void Close() //The method that executes Closed EventHandler
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
