using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs
{
    /// <summary>
    /// Interaction logic for AddTaskModalDialog.xaml
    /// </summary>
    public partial class AddTaskModalDialog : Window
    {
        public AddTaskModalDialog()
        {
            InitializeComponent();
        }

        private void AddTaskModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
