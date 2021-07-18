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
            this.Owner = Application.Current.MainWindow;
        }

        private void DialogWindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
