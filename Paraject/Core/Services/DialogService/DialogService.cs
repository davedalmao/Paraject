using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.Views.ModalDialogs.MessageBoxes;

namespace Paraject.Core.Services.DialogService
{
    public class DialogService : IDialogService
    {
        public T OpenDialog<T>(DialogBaseViewModel<T> viewModel)
        {
            IDialogWindow window = new DialogWindow
            {
                DataContext = viewModel
            };
            window.ShowDialog();
            return viewModel.DialogResult;
        }
    }
}
