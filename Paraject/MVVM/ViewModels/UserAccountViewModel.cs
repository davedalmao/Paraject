using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using System;
using System.Windows;

namespace Paraject.MVVM.ViewModels
{
    public class UserAccountViewModel
    {
        private readonly UserAccountRepository _userAccountRepository;

        public UserAccount CurrentUserAccount { get; set; }

        public void Add()
        {
            try
            {
                bool isSaved = _userAccountRepository.Add(CurrentUserAccount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Get()
        {
            UserAccount userAccount = _userAccountRepository.Get(CurrentUserAccount.Id);

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
    }
}
