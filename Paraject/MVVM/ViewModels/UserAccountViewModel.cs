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
            Get();
            //UpdateCommand = new DelegateCommand(Update);
            //DeleteCommand = new DelegateCommand(Delete);
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
                bool isUpdate = _userAccountRepository.Update(CurrentUserAccount);
                //after update change the values of the textboxes (username and password)
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
