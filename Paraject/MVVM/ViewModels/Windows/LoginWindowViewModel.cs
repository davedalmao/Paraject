using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.Views.Windows;
using System;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.Windows
{
    public class LoginWindowViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly UserAccountRepository _userAccountRepository;
        private DelegateCommand _signUpWindowRedirectCommand;
        public event EventHandler Closed; //The Window (LoginWindow) closes itself when this event is executed

        public LoginWindowViewModel()
        {
            _dialogService = new DialogService();
            _userAccountRepository = new UserAccountRepository();
            CurrentUserAccount = new UserAccount();
            LoginCommand = new DelegateCommand(Login);
        }

        #region Properties
        public UserAccount CurrentUserAccount { get; set; }

        public ICommand SignUpWindowRedirectCommand => _signUpWindowRedirectCommand ??= new DelegateCommand(ShowSignupWindow);
        public ICommand LoginCommand { get; }
        #endregion 

        #region Methods
        private void Login()
        {
            if (ValidateInput())
            {
                ShowMainWindow();
            }

            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry",
                                              "Check your inputs: \n\n1. User does not exist \n2. Incorrect Username \n3. Incorrect Password", Icon.InvalidUser));
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

        private void ShowSignupWindow()
        {
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.Show();
            Close(); //Closes LoginWindow when SignupWindow is present
        }
        private void ShowMainWindow()
        {
            UserAccount userToLogin = _userAccountRepository.GetByUsername(CurrentUserAccount.Username);
            MainWindow mainWindow = new(userToLogin);
            mainWindow.Show();
            Close(); //Closes LoginWindow when MainWindow is present
        }

        private void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
