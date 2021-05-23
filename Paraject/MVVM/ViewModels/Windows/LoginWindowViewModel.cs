using Paraject.Core.Commands;
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
        public NavigationCommand SignupWindowViewCommand { get; set; }

    }
}
