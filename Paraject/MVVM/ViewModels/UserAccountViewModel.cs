using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.Views.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class UserAccountViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly UserAccountRepository _userAccountRepository;

        public UserAccountViewModel(UserAccount currentUserAccount)
        {
            _dialogService = new DialogService();
            _userAccountRepository = new UserAccountRepository();
            CurrentUserAccount = currentUserAccount;

            UpdateCurrentUserCommand = new DelegateCommand(Update);
            DeleteCurrentUserCommand = new DelegateCommand(Delete);

        }

        #region Properties
        public UserAccount CurrentUserAccount { get; set; }

        public ICommand UpdateCurrentUserCommand { get; }
        public ICommand DeleteCurrentUserCommand { get; }
        #endregion

        #region Methods 
        public void Update()
        {
            if (!string.IsNullOrWhiteSpace(CurrentUserAccount.Username) && !string.IsNullOrWhiteSpace(CurrentUserAccount.Password))
            {
                bool idExists = _userAccountRepository.IdExistsInDatabase(CurrentUserAccount.Id);
                if (idExists)
                {
                    UpdateOperationResult();
                }
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "Your account should have a username AND password.", Icon.InvalidUser));
            }
        }
        private void UpdateOperationResult()
        {
            bool isUpdate = _userAccountRepository.Update(CurrentUserAccount);

            if (isUpdate)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", "User Account Updated Successfully!", Icon.ValidUser));
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot update your Account.", Icon.InvalidUser));
            }
        }

        public void Delete()
        {
            DialogResults result = _dialogService.OpenDialog(new YesNoMessageBoxViewModel("Delete Operation",
            "Do you want to DELETE your account? \n\nAll of your Projects, Project Ideas, Project Tasks, Project Task's Subtasks, and Project Notes will be also deleted.",
           Icon.User));

            if (result == DialogResults.Yes)
            {
                DeleteUserAccount();
            }
        }
        private void DeleteUserAccount()
        {
            bool isDeleted = _userAccountRepository.Delete(CurrentUserAccount.Id);

            if (isDeleted)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Delete Operation", $"Your account {CurrentUserAccount.Username} is now Deleted!", Icon.ValidUser));
                ShowLoginWindow();
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured while deleting your Account, please try again.", Icon.InvalidUser));
            }
        }

        private void ShowLoginWindow()
        {
            LoginWindow loginWindow = new();
            loginWindow.Show();
            CloseMainWindow();
        }
        private void CloseMainWindow()
        {
            if (CloseWindow.WinObject != null)
                CloseWindow.CloseParent();
        }
        #endregion
    }
}
