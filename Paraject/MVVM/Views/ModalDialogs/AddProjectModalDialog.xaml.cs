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
        }

        private void AddProjectModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
