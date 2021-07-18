using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.Views.ModalDialogs.MessageBoxes
{
    /// <summary>
    /// Interaction logic for YesNoMessageBox.xaml
    /// </summary>
    public partial class YesNoMessageBox : Window
    {
        public YesNoMessageBox()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        private void YesNoMessageBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }
    }
}
