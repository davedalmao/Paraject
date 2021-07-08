using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TaskDetailsViewModel : BaseViewModel
    {
        private readonly TaskRepository _taskRepository;
        private readonly Action _refreshTaskCollection;
        private readonly TasksViewModel _tasksViewModel;

        /// <summary>
        /// This displays the details of the selectedTask
        /// </summary>
        /// <param name="refreshTaskCollection">refreshes the Collection in the ChildView (TasksTodoView/CompletedTasksView) after a certain action is invoked</param>
        /// <param name="tasksViewModel">this is passed to save the UI state of TasksView when navigating back to it</param>
        /// <param name="selectedTask">the selected task in TasksTodoView/CompletedTasksview</param>
        public TaskDetailsViewModel(Action refreshTaskCollection, TasksViewModel tasksViewModel, Task selectedTask)
        {
            _taskRepository = new TaskRepository();
            _refreshTaskCollection = refreshTaskCollection;

            _tasksViewModel = tasksViewModel;
            CurrentTask = selectedTask;
            UpdateTaskCommand = new DelegateCommand(Update);
            DeleteTaskCommand = new DelegateCommand(Delete);
            ShowAddSubtaskModalDialogCommand = new DelegateCommand(ShowAddSubtaskModalDialog);
            NavigateBackToTasksViewCommand = new DelegateCommand(NavigateBackToTasksView);
            NavigateToSubtasksViewCommand = new DelegateCommand(NavigateToSubtasksView);
        }

        #region Properties
        public Task CurrentTask { get; set; }

        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand ShowAddSubtaskModalDialogCommand { get; }
        public ICommand NavigateBackToTasksViewCommand { get; }
        public ICommand NavigateToSubtasksViewCommand { get; }
        #endregion

        #region Methods
        private void Update()
        {
            if (!string.IsNullOrWhiteSpace(CurrentTask.Subject))
            {
                bool isUpdated = _taskRepository.Update(CurrentTask);
                if (isUpdated)
                {
                    MessageBox.Show("Task updated successfully");
                }
                else
                {
                    MessageBox.Show("Error occured, cannot update the task");
                }
            }
            else
            {
                MessageBox.Show("A Task should have a subject");
            }
        }
        private void Delete()
        {
            MessageBoxResult Result = MessageBox.Show("Do you want to DELETE this task?", "Delete Operation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                DeleteProject();
            }
        }
        private void DeleteProject()
        {
            bool isDeleted = _taskRepository.Delete(CurrentTask.Id);
            if (isDeleted)
            {
                MessageBox.Show("Task deleted successfully");

                //redirect to TasksView (parent View) after a successful DELETE operation, and
                //refreshes the Tasks Collection in TasksTodoView/CompletedTasksView (child View/s of TasksView) with the new records
                _refreshTaskCollection();
                MainWindowViewModel.CurrentView = _tasksViewModel;
            }
            else
            {
                MessageBox.Show("An error occurred, cannot delete task");
            }
        }
        private void ShowAddSubtaskModalDialog()
        {
            MainWindowViewModel.Overlay = true;

            AddSubtaskModalDialogViewModel addSubtaskModalDialogViewModel = new AddSubtaskModalDialogViewModel(CurrentTask.Id);

            AddSubtaskModalDialog addSubtaskModalDialog = new AddSubtaskModalDialog();
            addSubtaskModalDialog.DataContext = addSubtaskModalDialogViewModel;
            addSubtaskModalDialog.Show();
        }
        private void NavigateBackToTasksView()
        {
            _refreshTaskCollection();
            MainWindowViewModel.CurrentView = _tasksViewModel;
        }
        private void NavigateToSubtasksView()
        {
            SubtasksViewModel subtasksViewModel = new SubtasksViewModel(this, CurrentTask);

            MainWindowViewModel.CurrentView = subtasksViewModel;
        }
        #endregion
    }
}
