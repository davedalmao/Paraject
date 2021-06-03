using System;
using System.Windows;

namespace Paraject.Core.Utilities
{
    public static class CloseWindow
    {
        public static Window WinObject;

        /// <summary>
        /// This method is used to close its PARENT WINDOW
        /// </summary>
        public static void CloseParent()
        {
            try
            {
                WinObject.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CloseWindow in Utilities folder: {ex}");
            }
        }
    }
}
