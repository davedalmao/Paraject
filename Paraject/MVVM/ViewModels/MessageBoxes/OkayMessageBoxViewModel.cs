using Paraject.Core.Commands;
using Paraject.Core.Converters;
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

        public OkayMessageBoxViewModel(string title, string message, Icon iconSource) : base(message, title, iconSource)
        {
            Title = title;
            Message = message;
            IconSource = iconSource.GetDescription();
            OkayCommand = new RelayCommand<IDialogWindow>(Okay);
        }

        #region Properties
        public Action Close { get; set; }

        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(CloseWindow);
        public ICommand OkayCommand { get; private set; }
        #endregion

        #region Methods
        private void Okay(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.Okay);
        }
        public void CloseWindow()
        {
            Close?.Invoke();
        }
        #endregion
    }
}
