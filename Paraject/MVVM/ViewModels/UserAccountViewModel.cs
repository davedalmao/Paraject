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

            GetUserAccountDetails();
        }

        public UserAccount CurrentUserAccount { get; set; }
        public ICommand UpdateCurrentUserCommand { get; }
        public ICommand DeleteCurrentUserCommand { get; }

        #region Methods 
        public void GetUserAccountDetails()
        {
            UserAccount userAccount = _userAccountRepository.GetByUsername(CurrentUserAccount.Username);

            if (userAccount is not null)
            {
                CurrentUserAccount.Id = userAccount.Id;
                CurrentUserAccount.Username = userAccount.Username;
                CurrentUserAccount.Password = userAccount.Password;
                CurrentUserAccount.DateCreated = userAccount.DateCreated;
            }
            else
            {
                MessageBox.Show("User Account not Found!");
            }
        }

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
                MessageBoxResult Result = MessageBox.Show("Do you want to DELETE your account?", "Delete Operation", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
