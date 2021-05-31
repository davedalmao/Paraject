using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class UserAccountViewModel : BaseViewModel
    {
        private readonly UserAccountRepository _userAccountRepository;
        private readonly string _username;

        public UserAccountViewModel(string username)
        {
            _username = username;
            CurrentUserAccount = new UserAccount();
            _userAccountRepository = new UserAccountRepository();

            UpdateCommand = new DelegateCommand(Update);
            //DeleteCommand = new DelegateCommand(Delete);

            Get();
        }
        #region Properties
        public UserAccount CurrentUserAccount { get; set; }
        public ICommand GetCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        #endregion 

        #region Get, Update, Delete
        public void Get()
        {
            UserAccount userAccount = _userAccountRepository.Get(_username);

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
                    UpdateCurrentUser();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void UpdateCurrentUser()
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
                bool isDeleted = _userAccountRepository.Delete(CurrentUserAccount.Id);
                //after deleting account, popup a message box, then redirect to login page
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
    }
}
