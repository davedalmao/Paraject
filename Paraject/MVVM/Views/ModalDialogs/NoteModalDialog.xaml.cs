using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs
{
    /// <summary>
    /// Interaction logic for NoteModalDialog.xaml
    /// </summary>
    public partial class NoteModalDialog : Window
    {
        public NoteModalDialog()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        private void NoteModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
