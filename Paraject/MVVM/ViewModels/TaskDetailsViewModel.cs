using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TaskDetailsViewModel : BaseViewModel
    {
        private readonly TaskRepository _taskRepository;
        private TasksTodoViewModel _taskTodoViewModel;
        private TasksViewModel _tasksViewModel;

        public TaskDetailsViewModel(TasksTodoViewModel tasksTodoViewModel, TasksViewModel tasksViewModel, Task selectedTask)
        {
            _taskRepository = new TaskRepository();
            _taskTodoViewModel = tasksTodoViewModel;
            _tasksViewModel = tasksViewModel;
            CurrentTask = selectedTask;
            UpdateTaskCommand = new DelegateCommand(Update);
            DeleteTaskCommand = new DelegateCommand(Delete);
        }

        #region Properties
        public Task CurrentTask { get; set; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
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

                //redirect to TasksView after a successful DELETE operation, and refresh the View in TasksTodoView (child View of TasksView) with the new records
                _taskTodoViewModel.DisplayAllFilteredTasks();
                MainWindowViewModel.CurrentView = _tasksViewModel;
            }
            else
            {
                MessageBox.Show("An error occurred, cannot delete task");
            }
        }
        #endregion
    }
}
