using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paraject.Core.CustomControlClasses
{
    /// <summary>
    /// Sidebar menu item
    /// </summary>
    class MenuItem : RadioButton
    {
        public string DefaultImageUriString
        {
            get { return (string)GetValue(DefaultImageUriStringProperty); }
            set { SetValue(DefaultImageUriStringProperty, value); }
        }
        public static readonly DependencyProperty DefaultImageUriStringProperty =
            DependencyProperty.Register("DefaultImageUriString", typeof(string), typeof(MenuItem), new PropertyMetadata(null));

        public string SelectedImageUriString
        {
            get { return (string)GetValue(SelectedImageUriStringProperty); }
            set { SetValue(SelectedImageUriStringProperty, value); }
        }
        public static readonly DependencyProperty SelectedImageUriStringProperty =
            DependencyProperty.Register("SelectedImageUriString", typeof(string), typeof(MenuItem), new PropertyMetadata(null));
    }
}
