using System.Windows;

namespace Paraject.Core.Utilities
{
    public static class WindowProperties
    {

        // Using a DependencyProperty as the backing store for CloseWindow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseWindowProperty =
            DependencyProperty.RegisterAttached("CloseWindow", typeof(bool), typeof(WindowProperties),
                new UIPropertyMetadata(false, (d, e) =>
                {
                    if (d is Window w && (bool)e.NewValue)
                    {
                        w.DialogResult = true;
                        w.Close();
                    }
                }));

        public static bool GetCloseWindow(DependencyObject obj)
        {
            return (bool)obj.GetValue(CloseWindowProperty);
        }

        public static void SetCloseWindow(DependencyObject obj, bool value)
        {
            obj.SetValue(CloseWindowProperty, value);
        }

    }
}
