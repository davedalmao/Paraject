using Paraject.Core.Commands;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class SubtasksViewModel : BaseViewModel
    {
        private readonly TaskDetailsViewModel _taskDetailsViewModel;

        public SubtasksViewModel(TaskDetailsViewModel taskDetailsViewModel)
        {
            NavigateBackToTaskDetailsViewCommand = new DelegateCommand(NavigateBackToTaskDetailsView);
            _taskDetailsViewModel = taskDetailsViewModel;
        }

        public ICommand NavigateBackToTaskDetailsViewCommand { get; }

        public void NavigateBackToTaskDetailsView()
        {
            MainWindowViewModel.CurrentView = _taskDetailsViewModel;
        }
    }
}
