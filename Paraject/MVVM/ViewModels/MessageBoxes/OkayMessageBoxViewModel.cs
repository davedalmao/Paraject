using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Services.DialogService;
using Paraject.Core.Utilities;
using System;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.MessageBoxes
{
    public class OkayMessageBoxViewModel : DialogBaseViewModel<DialogResults>, ICloseWindows
    {
        private ICommand _closeCommand;

        public OkayMessageBoxViewModel(string title, string message, string iconSource) : base(message, title, iconSource)
        {
            Title = title;
            Message = message;
            IconSource = iconSource;
            OkayCommand = new RelayCommand<IDialogWindow>(Okay);
        }

        public Action Close { get; set; }
        public ICommand CloseCommand => _closeCommand ??= new DelegateCommand(CloseWindow);
        public ICommand OkayCommand { get; private set; }

        private void Okay(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.Okay);
        }
        public void CloseWindow()
        {
            Close?.Invoke();
        }
    }
}
