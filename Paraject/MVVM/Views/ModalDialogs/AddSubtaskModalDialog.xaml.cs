using System.Windows;

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
            MouseDown += delegate { DragMove(); };
            this.Owner = Application.Current.MainWindow;
        }
    }
}
