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
        public DelegateCommand SignupWindowViewCommand { get; set; }

        public LoginWindowViewModel()
        {
            SignupWindowViewCommand = new DelegateCommand(ShowSignupWindow);
        }
        public void ShowSignupWindow()
        {
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.Show();
        }
    }
}
