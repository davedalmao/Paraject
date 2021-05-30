using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.Views.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.Windows
{
    public class SignupWindowViewModel : BaseViewModel
    {
        private readonly UserAccountRepository _userAccountRepository;
        private DelegateCommand _loginWindowRedirectCommand;
        public event EventHandler Closed; //The Window (LoginWindow) closes itself when this event is executed

        public SignupWindowViewModel()
        {
            _userAccountRepository = new UserAccountRepository();
            AddCommand = new DelegateCommand(Add);
            CurrentUserAccount = new UserAccount();
        }

        #region Properties
        public UserAccount CurrentUserAccount { get; set; }
        public string InitialPassword { get; set; }
        public ICommand AddCommand { get; }
        public ICommand LoginWindowRedirectCommand
        {
            get { return _loginWindowRedirectCommand ??= new DelegateCommand(ShowLoginWindow); }
        }
        #endregion

        #region Methods


        public void Add()
        {
            try
            {
                //MessageBox.Show($"Username: {CurrentUserAccount.Username} \nInitialPassword: {InitialPassword} \nPassword: {CurrentUserAccount.Password}");
                if (ValidateInput())
                {
                    bool isSaved = _userAccountRepository.Add(CurrentUserAccount);

                    if (isSaved)
                    {
                        ShowMainWindow();
                    }
                }

                else
                {
                    MessageBox.Show("Check your inputs: \n1. Passwords doesn't match \n2. No username in the input");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ValidateInput()
        {
            bool isValid = false;
            // Check if Username, initial Password, and confirm Password is not blank
            if (!string.IsNullOrWhiteSpace(CurrentUserAccount.Username) && !string.IsNullOrWhiteSpace(InitialPassword) && !string.IsNullOrWhiteSpace(CurrentUserAccount.Password))
            {
                //if Username, initial Password, and confirm Password is not blank:
                return InitialPassword.Equals(CurrentUserAccount.Password); //Check if initial password and confirm password is the same
            }
            return isValid;
        }


        public void ShowLoginWindow()
        {
            LoginWindow loginWindow = new();
            loginWindow.Show();
            Close(); //Closes SignupWindow when LoginWindow is present
        }


        public void ShowMainWindow()
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close(); //Closes SignupWindow when MainWindow is present
        }

        private void Close() //The method that executes Closed EventHandler
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}


