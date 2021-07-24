using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class SubtasksViewModel : BaseViewModel
    {
        private readonly Action _refreshTaskCollection;
        private readonly TasksViewModel _tasksViewModel;

        public SubtasksViewModel(Action refreshTaskCollection, TasksViewModel tasksViewModel, Task currentTask, Project parentProject)
        {
            _refreshTaskCollection = refreshTaskCollection;
            _tasksViewModel = tasksViewModel;
            CurrentTask = currentTask;

            AllSubtasksVM = new AllSubtasksViewModel("SubtasksTodo", true, currentTask);
            TaskDetailsVM = new TaskDetailsViewModel(refreshTaskCollection, tasksViewModel, currentTask, parentProject);

            CurrentChildView = AllSubtasksVM;

            NavigateBackToTasksViewCommand = new DelegateCommand(NavigateBackToTasksView);
            SubtasksFilterCommand = new ParameterizedDelegateCommand(DisplayFilteredSubtasks);
            TaskDetailsCommand = new ParameterizedDelegateCommand(o => { CurrentChildView = TaskDetailsVM; });
        }

        #region Properties
        public Task CurrentTask { get; set; } // I used a Property here instead of an int field for Id because I will bind CurrentTask.Category to the UI
        public object CurrentChildView { get; set; }

        //Child Views
        public AllSubtasksViewModel AllSubtasksVM { get; set; } //Subtasks Todo and Completed Subtasks tab
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
            AllSubtasksVM = new AllSubtasksViewModel(filterType.ToString(), !CompletedSubtasksButtonIsChecked, CurrentTask);
            CurrentChildView = AllSubtasksVM;
        }
        #endregion
    }
}
