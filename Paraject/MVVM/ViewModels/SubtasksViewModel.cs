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

            AllSubtasksVM = new AllSubtasksViewModel(true, currentTask.Id);
            TaskDetailsVM = new TaskDetailsViewModel(taskDetailsViewModel, currentTask);

            CurrentChildView = AllSubtasksVM;

            NavigateBackToTaskDetailsViewCommand = new DelegateCommand(NavigateBackToTaskDetailsView);
            SubtasksFilterCommand = new ParameterizedDelegateCommand(DisplayFilteredSubtasks);
            TaskDetailsCommand = new ParameterizedDelegateCommand(o => { CurrentChildView = TaskDetailsVM; });
        }

        #region Properties
        public Task CurrentTask { get; set; }
        public string CurrentTaskCategory => $"[ {CurrentTask.Category} ]";

        public object CurrentChildView { get; set; }

        //Child Views
        public AllSubtasksViewModel AllSubtasksVM { get; set; }
        public TaskDetailsViewModel TaskDetailsVM { get; set; }

        public bool CompletedSubtasksIsChecked { get; set; }
        public bool ComboBoxesRowVisibility { get; set; } = true;

        public ICommand NavigateBackToTaskDetailsViewCommand { get; }
        public ICommand SubtasksFilterCommand { get; }
        public ICommand TaskDetailsCommand { get; }
        #endregion

        #region Methods
        private void NavigateBackToTaskDetailsView()
        {
            MainWindowViewModel.CurrentView = _tasksViewModel;
        }
        private void DisplayFilteredSubtasks(object filter)
        {
            bool isCompletedButtonChecked = filter.ToString() == "SubtasksTodo";

            AllSubtasksVM = new AllSubtasksViewModel(isCompletedButtonChecked, CurrentTask.Id);
            CurrentChildView = AllSubtasksVM;
        }
        #endregion
    }
}
