using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs
{
    /// <summary>
    /// Interaction logic for ProjectIdeaDetailsModalDialog.xaml
    /// </summary>
    public partial class ProjectIdeaDetailsModalDialog : Window
    {
        public ProjectIdeaDetailsModalDialog()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        private void ProjectIdeaDetailsModalDialogMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
