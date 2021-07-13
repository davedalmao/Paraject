using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs
{
    /// <summary>
    /// Interaction logic for AddNoteModalDialog.xaml
    /// </summary>
    public partial class AddNoteModalDialog : Window
    {
        public AddNoteModalDialog()
        {
            InitializeComponent();
        }

        private void AddNoteModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
