using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs
{
    /// <summary>
    /// Interaction logic for AddSubtaskModalDialog.xaml
    /// </summary>
    public partial class AddSubtaskModalDialog : Window
    {
        public AddSubtaskModalDialog()
        {
            InitializeComponent();
        }

        private void AddSubtaskModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
