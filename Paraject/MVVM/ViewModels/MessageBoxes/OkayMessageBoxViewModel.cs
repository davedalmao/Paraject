using Paraject.Core.Commands;
using Paraject.Core.Utilities;
using Paraject.MVVM.ViewModels.Windows;
using System;

namespace Paraject.MVVM.ViewModels.MessageBoxes
{
    public class OkayMessageBoxViewModel : ICloseWindows
    {
        private DelegateCommand _closeCommand;

        public OkayMessageBoxViewModel(string title, string message)
        {
            Title = title;
            Message = message;

        }

        public string Title { get; set; }
        public string Message { get; set; }

        public Action Close { get; set; }
        public DelegateCommand CloseCommand => _closeCommand ??= new DelegateCommand(CloseWindow);

        public void CloseWindow()
        {
            Close?.Invoke();
            MainWindowViewModel.Overlay = false;
        }
    }
}
