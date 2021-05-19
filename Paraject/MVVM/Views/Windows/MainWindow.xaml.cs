using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paraject.MVVM.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static readonly DependencyProperty DefDashboardImage = DependencyProperty.Register("ImageUri", typeof(string), typeof(MainWindow));
        //public static readonly DependencyProperty SelDashboardImage = DependencyProperty.Register("ImageUri", typeof(string), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            //DataContext = this;
            //DefaultDashboardImage = "/UiDesign/Images/SidebarIcons/logoout.png";
        }

        //public string DefaultDashboardImage
        //{
        //    get { return (string)GetValue(DefDashboardImage); }
        //    set
        //    {
        //        if (radioBtnDashboard.Foreground == (SolidColorBrush)(new BrushConverter().ConvertFrom("#F54342")))
        //        {

        //        }
        //        else
        //        {
        //            SetValue(DefDashboardImage, value);
        //        }
        //    }
        //}

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
