using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;

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
            MouseDown += delegate { DragMove(); };
        }


        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Minimized)
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void CloseMainWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {

            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }
    }
}
