using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class SubtasksViewModel : BaseViewModel
    {
        private readonly TasksViewModel _tasksViewModel;

        public SubtasksViewModel(TasksViewModel taskDetailsViewModel, Task currentTask)
        {
            _tasksViewModel = taskDetailsViewModel;
            CurrentTask = currentTask;

            NavigateBackToTaskDetailsViewCommand = new DelegateCommand(NavigateBackToTaskDetailsView);

        }

        #region Properties
        public Task CurrentTask { get; set; }
        public string CurrentTaskCategory => $"[ {CurrentTask.Category} ]";

        public bool CompletedSubtasksIsChecked { get; set; }
        public bool ComboBoxesRowVisibility { get; set; } = true;

        public ICommand NavigateBackToTaskDetailsViewCommand { get; }
        #endregion

        #region Methods
        private void NavigateBackToTaskDetailsView()
        {
            MainWindowViewModel.CurrentView = _tasksViewModel;
        }
        #endregion
    }
}
