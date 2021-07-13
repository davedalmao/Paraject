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
            this.Owner = Application.Current.MainWindow;
        }

        private void NoteDetailsModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
