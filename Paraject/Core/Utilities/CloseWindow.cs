using Paraject.Core.Enums;
using Paraject.Core.Services.DialogService;
using Paraject.MVVM.ViewModels.MessageBoxes;
using System;
using System.Windows;

namespace Paraject.Core.Utilities
{
    public static class CloseWindow
    {
        private static readonly IDialogService _dialogService;
        public static Window WinObject;

        /// <summary>
        /// This method is used to close its PARENT WINDOW
        /// </summary>

        static CloseWindow()
        {
            _dialogService = new DialogService();
        }


        public static void CloseParent()
        {
            try
            {
                WinObject.Close();
            }
            catch (Exception ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ex}", Icon.Error));
            }
        }
    }
}
