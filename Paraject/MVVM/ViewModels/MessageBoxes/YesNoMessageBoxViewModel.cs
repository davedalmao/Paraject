using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Services.DialogService;
using Paraject.Core.Utilities;
using System;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.MessageBoxes
{
    public class YesNoMessageBoxViewModel : DialogBaseViewModel<DialogResults>, ICloseWindows
    {
        private ICommand _closeCommand;

        public YesNoMessageBoxViewModel(string title, string message, string iconSource) : base(message, title, iconSource)
        {
            Title = title;
            Message = message;
            IconSource = iconSource;

            YesCommand = new RelayCommand<IDialogWindow>(Yes);
            NoCommand = new RelayCommand<IDialogWindow>(No);
        }

        #region Properties
        public Action Close { get; set; }

        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }
        public ICommand CloseCommand => _closeCommand ??= new DelegateCommand(CloseWindow);
        #endregion

        #region Methods
        private void Yes(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.Yes);
        }
        private void No(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.No);
        }
        public void CloseWindow()
        {
            Close?.Invoke();
        }
        #endregion
    }
}
