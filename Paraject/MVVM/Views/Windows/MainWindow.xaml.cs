using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowViewModel _viewModel;
        public MainWindow(UserAccount currentUserAccount)
        {
            InitializeComponent();
            CloseWindow.WinObject = this;

            _viewModel = new MainWindowViewModel(currentUserAccount);
            DataContext = _viewModel;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
