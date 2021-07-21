using Paraject.Core.Converters;
using Paraject.Core.Enums;
using Paraject.Core.Services.DialogService;

namespace Paraject.MVVM.ViewModels.MessageBoxes
{
    public abstract class DialogBaseViewModel<T>
    {
        public DialogBaseViewModel(string title, string message, Icon iconSource)
        {
            Title = title;
            Message = message;
            IconSource = iconSource.GetDescription();
        }
        public string Title { get; set; }
        public string Message { get; set; }
        public string IconSource { get; set; }

        public T DialogResult { get; set; }

        public void CloseDialogWithResult(IDialogWindow dialog, T result)
        {
            DialogResult = result;

            if (dialog != null)
            {
                dialog.DialogResult = true;
            }
        }
    }
}
