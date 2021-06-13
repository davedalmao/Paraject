using Paraject.MVVM.ViewModels.Windows;
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
        private void CloseModal(object sender, RoutedEventArgs e)
        {
            //Remove the Overlay from MainWindow before closing the dialog
            MainWindowViewModel.Overlay = false;
            Close();
        }
    }
}
