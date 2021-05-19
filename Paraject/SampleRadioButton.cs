using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paraject
{
    public class SampleRadioButton : RadioButton
    {
        public string NormalImageUriString
        {
            get { return (string)GetValue(NormalImageUriStringProperty); }
            set { SetValue(NormalImageUriStringProperty, value); }
        }
        public static readonly DependencyProperty NormalImageUriStringProperty =
            DependencyProperty.Register("NormalImageUriString", typeof(string), typeof(SampleRadioButton), new PropertyMetadata(null));

        public string CheckedImageUriString
        {
            get { return (string)GetValue(CheckedImageUriStringProperty); }
            set { SetValue(CheckedImageUriStringProperty, value); }
        }
        public static readonly DependencyProperty CheckedImageUriStringProperty =
            DependencyProperty.Register("CheckedImageUriString", typeof(string), typeof(SampleRadioButton), new PropertyMetadata(null));
    }

}
