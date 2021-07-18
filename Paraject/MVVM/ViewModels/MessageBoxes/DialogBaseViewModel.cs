using Paraject.Core.Services.DialogService;

namespace Paraject.MVVM.ViewModels.MessageBoxes
{
    public abstract class DialogBaseViewModel<T>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string IconSource { get; set; }

        public T DialogResult { get; set; }


        public DialogBaseViewModel(string title, string message, string iconSource)
        {
            Title = title;
            Message = message;
            IconSource = iconSource;
        }

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
