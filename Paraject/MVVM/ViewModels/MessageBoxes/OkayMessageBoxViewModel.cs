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
        private DelegateCommand _closeCommand;

        public OkayMessageBoxViewModel(string title, string message) : base(message, title)
        {
            Title = title;
            Message = message;
            OkayCommand = new RelayCommand<IDialogWindow>(Okay);
        }


        public string Icon => "/UiDesign/Images/Logo/defaultProjectLogo.png";
        public Action Close { get; set; }
        public DelegateCommand CloseCommand => _closeCommand ??= new DelegateCommand(CloseWindow);
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
