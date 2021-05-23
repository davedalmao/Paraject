using Paraject.Core.Commands;
using Paraject.MVVM.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraject.MVVM.ViewModels.Windows
{
    class SignupWindowViewModel : BaseViewModel
    {
        private DelegateCommand _loginWindowRedirectCommand;
        public event EventHandler Closed; //The Window (LoginWindow) closes itself when this event is executed

        /// <summary>
        /// The command that Shows LoginWindow and Closes SignupWindow
        /// </summary>
        public DelegateCommand LoginWindowRedirectCommand
        {
            get { return _loginWindowRedirectCommand ??= new DelegateCommand(ShowLoginWindow); }
        }


        public void ShowLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close(); //Closes SignupWindow when LoginWindow is present
        }

        //The method that executes Closed EventHandler
        private void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
