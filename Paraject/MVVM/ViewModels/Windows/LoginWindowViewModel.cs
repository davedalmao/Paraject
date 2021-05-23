using Paraject.Core.Commands;
using Paraject.MVVM.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paraject.MVVM.ViewModels.Windows
{
    class LoginWindowViewModel : BaseViewModel
    {
        private DelegateCommand _signUpWindowRedirectCommand;
        public event EventHandler Closed; //The Window (LoginWindow) closes itself when this event is executed

        public DelegateCommand SignUpWindowRedirectCommand
        {
            get
            {
                return _signUpWindowRedirectCommand ??= new DelegateCommand(ShowSignupWindow);
            }
        }


        public void ShowSignupWindow()
        {
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.Show();
            CloseThisWindow();
        }

        //The method that executes Closed EventHandler
        private void CloseThisWindow()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
