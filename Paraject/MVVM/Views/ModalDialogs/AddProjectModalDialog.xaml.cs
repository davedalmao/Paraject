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
            DisplayWatermark();
        }

        private void CloseModal(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DisplayWatermark()
        {
            if (DeadlineDatePicker.Text.Length == 0 || DeadlineDatePicker.SelectedDate is null)
            {
                DeadlineDatePicker.Text = "re";
            }
        }
    }
}
