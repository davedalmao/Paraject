using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs
{
    /// <summary>
    /// Interaction logic for SubtaskDetailsModalDialog.xaml
    /// </summary>
    public partial class SubtaskDetailsModalDialog : Window
    {
        public SubtaskDetailsModalDialog()
        {
            InitializeComponent();
        }

        private void SubtaskDetailsModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
