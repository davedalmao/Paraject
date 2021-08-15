using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs
{
    /// <summary>
    /// Interaction logic for NoteDetailsModalDialog.xaml
    /// </summary>
    public partial class NoteDetailsModalDialog : Window
    {
        public NoteDetailsModalDialog()
        {
            InitializeComponent();
        }

        private void NoteDetailsModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
