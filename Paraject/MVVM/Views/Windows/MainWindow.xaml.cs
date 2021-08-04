using Paraject.Core.Services.DialogService;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.ViewModels.Windows;
using Squirrel;
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
        UpdateManager _updateManager;
        private readonly IDialogService _dialogService;

        public MainWindow(UserAccount currentUserAccount)
        {
            InitializeComponent();
            _dialogService = new DialogService();
            CloseWindow.WinObject = this;

            _viewModel = new MainWindowViewModel(currentUserAccount);
            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _updateManager = await UpdateManager.GitHubUpdateManager(@"https://github.com/paraJdox1/Paraject");
            CheckForAppUpdates();
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

        private void MainWindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }

        private async void CheckForAppUpdates()
        {
            var updateInfo = await _updateManager.CheckForUpdate();

            if (updateInfo.ReleasesToApply.Count > 0)
            {
                await _updateManager.UpdateApp();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("App Update", "App Updated Successfully!", Core.Enums.Icon.Error));
            }
        }
    }
}
