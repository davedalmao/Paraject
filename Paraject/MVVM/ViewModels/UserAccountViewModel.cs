using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.Views.Windows;
using System;
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
            try
            {
                bool idExists = _userAccountRepository.IdExistsInDatabase(CurrentUserAccount.Id);
                if (idExists)
                {
                    UpdateUserAccount();
                }
            }
            catch (Exception ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ex}", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            }
        }
        private void UpdateUserAccount()
        {
            bool isUpdate = _userAccountRepository.Update(CurrentUserAccount);

            if (isUpdate)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", "User Account Updated Successfully!", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot update your User Account.", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            }
        }

        public void Delete()
        {
            DialogResults result = _dialogService.OpenDialog(new YesNoMessageBoxViewModel("Delete Operation",
            "Do you want to DELETE your account? \n\nAll of your Projects, Project Ideas, Project Tasks, Project Task's Subtasks, and Project Notes will be also deleted.",
            "/UiDesign/Images/Logo/defaultProjectLogo.png"));

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
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Delete Operation", $"Your account {CurrentUserAccount.Username} is now Deleted!", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
                ShowLoginWindow();
            }

            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, while deleting your Account, please try again.", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
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
