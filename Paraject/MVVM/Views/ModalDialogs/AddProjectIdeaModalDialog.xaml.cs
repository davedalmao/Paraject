using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs
{
    /// <summary>
    /// Interaction logic for AddProjectIdeaModalDialog.xaml
    /// </summary>
    public partial class AddProjectIdeaModalDialog : Window
    {
        public AddProjectIdeaModalDialog()
        {
            InitializeComponent();
        }

        private void AddProjectIdeaModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
