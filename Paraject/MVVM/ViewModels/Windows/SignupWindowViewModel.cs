using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.Views.Windows;
using System;
using System.Windows;

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
            AddCommand = new DelegateCommand(AddTest);
        }

        #region Properties
        /// <summary>
        /// The command that Shows LoginWindow and Closes SignupWindow
        /// </summary>
        public DelegateCommand LoginWindowRedirectCommand { get { return _loginWindowRedirectCommand ??= new DelegateCommand(ShowLoginWindow); } }

        public UserAccount CurrentUserAccount { get; set; }
        public DelegateCommand AddCommand { get; }
        #endregion

        #region Methods
        public void ShowLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close(); //Closes SignupWindow when LoginWindow is present
        }

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

        public void AddTest()
        {
            MessageBox.Show("fsd");
        }

        //The method that executes Closed EventHandler
        private void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
