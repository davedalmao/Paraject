using System.Windows;

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
            MouseDown += delegate { DragMove(); };
            this.Owner = Application.Current.MainWindow;
        }
    }
}
