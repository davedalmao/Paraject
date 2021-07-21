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
    public class SignupWindowViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly UserAccountRepository _userAccountRepository;
        private DelegateCommand _loginWindowRedirectCommand;
        public event EventHandler Closed; //The Window (LoginWindow) closes itself when this event is executed

        public SignupWindowViewModel()
        {
            _dialogService = new DialogService();
            _userAccountRepository = new UserAccountRepository();
            AddCommand = new DelegateCommand(Add);
            CurrentUserAccount = new UserAccount();
        }

        #region Properties
        public string InitialPassword { get; set; }
        public UserAccount CurrentUserAccount { get; set; }

        public ICommand AddCommand { get; }
        public ICommand LoginWindowRedirectCommand => _loginWindowRedirectCommand ??= new DelegateCommand(ShowLoginWindow);
        #endregion

        #region Methods
        private void Add()
        {
            if (ValidateInput())
            {
                AddUserAccountToDatabase();
            }

            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry",
                                             "Check your inputs: \n\n1. Passwords does not match \n2. No username in the input", Icon.InvalidUser));
            }

        }
        private void AddUserAccountToDatabase()
        {
            bool isSaved = _userAccountRepository.Add(CurrentUserAccount);

            if (isSaved)
            {
                ShowMainWindow();
            }
        }
        private bool ValidateInput()
        {
            bool isValid = false;

            if (!string.IsNullOrWhiteSpace(CurrentUserAccount.Username) &&
                !string.IsNullOrWhiteSpace(InitialPassword) &&
                !string.IsNullOrWhiteSpace(CurrentUserAccount.Password))
            {
                return InitialPassword.Equals(CurrentUserAccount.Password);
            }
            return isValid;
        }

        private void ShowLoginWindow()
        {
            LoginWindow loginWindow = new();
            loginWindow.Show();
            Close(); //Closes SignupWindow when LoginWindow is present
        }
        private void ShowMainWindow()
        {
            _dialogService.OpenDialog(new OkayMessageBoxViewModel("Welcome", "Account Created Successfully! \nPress Okay to Begin!", Icon.ValidUser));

            UserAccount userToLogin = _userAccountRepository.GetByUsername(CurrentUserAccount.Username);
            MainWindow mainWindow = new(userToLogin);
            mainWindow.Show();
            Close(); //Closes SignupWindow when MainWindow is present
        }

        private void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}


