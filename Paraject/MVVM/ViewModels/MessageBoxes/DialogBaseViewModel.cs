using Paraject.Core.Services.DialogService;

namespace Paraject.MVVM.ViewModels.MessageBoxes
{
    public abstract class DialogBaseViewModel<T>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public T DialogResult { get; set; }


        public DialogBaseViewModel(string title, string message)
        {
            Title = title;
            Message = message;
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
