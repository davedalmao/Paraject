using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.Views.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class UserAccountViewModel : BaseViewModel
    {
        private readonly UserAccountRepository _userAccountRepository;

        public UserAccountViewModel(UserAccount currentUserAccount)
        {
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
                MessageBox.Show(ex.ToString());
            }
        }
        private void UpdateUserAccount()
        {
            bool isUpdate = _userAccountRepository.Update(CurrentUserAccount);

            if (isUpdate)
            {
                MessageBox.Show("User Updated");
            }
            else
            {
                MessageBox.Show("Failed to Update User");

            }
        }

        public void Delete()
        {
            try
            {
                MessageBoxResult Result = MessageBox.Show("Do you want to DELETE your account? \n\nAll of your Project/s, Project Idea/s, Project Task/s, Project Task's Subtask/s, and Project Note/s will be also deleted.", "Delete Operation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.Yes)
                {
                    DeleteUserAccount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DeleteUserAccount()
        {
            bool isDeleted = _userAccountRepository.Delete(CurrentUserAccount.Id);

            if (isDeleted)
            {
                MessageBox.Show($"Your account {CurrentUserAccount.Username} is now deleted");
                ShowLoginWindow();
            }

            else
            {
                MessageBox.Show("An error occured when deleting your account, please try again");
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
