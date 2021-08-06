using Paraject.Core.Services.DialogService;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs.MessageBoxes
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, IDialogWindow
    {
        public DialogWindow()
        {
            InitializeComponent();

            if (Application.Current.MainWindow is not DialogWindow)
            {
                this.Owner = Application.Current.MainWindow;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            else
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        private void DialogWindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
