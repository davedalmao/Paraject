using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TaskContentViewModel : BaseViewModel
    {
        private readonly Action _refreshTaskCollection;
        private readonly ProjectContentViewModel _tasksViewModel;

        public TaskContentViewModel(Action refreshTaskCollection, ProjectContentViewModel tasksViewModel, Task currentTask, Project parentProject)
        {
            _refreshTaskCollection = refreshTaskCollection;
            _tasksViewModel = tasksViewModel;
            CurrentTask = currentTask;

            SubtasksVM = new SubtasksViewModel("SubtasksTodo", true, currentTask);
            TaskDetailsVM = new TaskDetailsViewModel(refreshTaskCollection, tasksViewModel, currentTask, parentProject);

            CurrentChildView = SubtasksVM;

            NavigateBackToTasksViewCommand = new RelayCommand(NavigateBackToTasksView);
            SubtasksFilterCommand = new RelayCommand<object>(DisplayFilteredSubtasks);
            TaskDetailsCommand = new RelayCommand<object>(o => { CurrentChildView = TaskDetailsVM; });
        }

        #region Properties
        public Task CurrentTask { get; set; } // I used a Property here instead of an int field for Id because I will bind CurrentTask.Category to the UI
        public object CurrentChildView { get; set; }

        //Child Views
        public SubtasksViewModel SubtasksVM { get; set; } //Subtasks Todo and Completed Subtasks tab
        public TaskDetailsViewModel TaskDetailsVM { get; set; }

        public bool CompletedSubtasksButtonIsChecked { get; set; } //Hides the input row in AllSubtasksView if false

        public ICommand NavigateBackToTasksViewCommand { get; }
        public ICommand SubtasksFilterCommand { get; }
        public ICommand TaskDetailsCommand { get; }
        #endregion

        #region Methods
        private void NavigateBackToTasksView()
        {
            _refreshTaskCollection();
            MainWindowViewModel.CurrentView = _tasksViewModel;
        }
        private void DisplayFilteredSubtasks(object filterType)
        {
            SubtasksVM = new SubtasksViewModel(filterType.ToString(), !CompletedSubtasksButtonIsChecked, CurrentTask);
            CurrentChildView = SubtasksVM;
        }
        #endregion
    }
}
