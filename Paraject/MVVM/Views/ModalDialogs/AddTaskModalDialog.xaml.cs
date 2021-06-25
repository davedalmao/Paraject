using System.Windows;

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
            MouseDown += delegate { DragMove(); };
            this.Owner = Application.Current.MainWindow;
        }
    }
}
