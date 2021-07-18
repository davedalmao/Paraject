using Paraject.MVVM.ViewModels.MessageBoxes;

namespace Paraject.Core.Services.DialogService
{
    public class DialogService : IDialogService
    {
        public T OpenDialog<T>(DialogBaseViewModel<T> viewModel)
        {
            //IDialogWindow window = new DialogWindow();
            //window.DataContext = viewModel;
            //window.ShowDialog();
            return viewModel.DialogResult;
        }
    }
}
