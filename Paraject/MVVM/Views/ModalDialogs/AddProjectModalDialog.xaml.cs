using Microsoft.Win32;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ProjectLogo.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
    }
}
