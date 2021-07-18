using Paraject.MVVM.ViewModels.MessageBoxes;

namespace Paraject.Core.Services.DialogService
{
    public interface IDialogService
    {
        T OpenDialog<T>(DialogBaseViewModel<T> viewModel);
    }
}
