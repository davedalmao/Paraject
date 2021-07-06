using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs
{
    /// <summary>
    /// Interaction logic for AddProjectModalDialog.xaml
    /// </summary>
    public partial class AddProjectModalDialog : Window
    {
        public AddProjectModalDialog()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        private void AddProjectModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
