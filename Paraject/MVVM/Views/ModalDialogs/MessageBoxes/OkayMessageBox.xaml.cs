using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs.MessageBoxes
{
    /// <summary>
    /// Interaction logic for OkayMessageBox.xaml
    /// </summary>
    public partial class OkayMessageBox : Window
    {
        public OkayMessageBox()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        private void OkayMessageBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
