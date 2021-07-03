using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TaskDetailsViewModel : BaseViewModel
    {
        private readonly TaskRepository _taskRepository;

        public TaskDetailsViewModel(Task selectedTask)
        {
            _taskRepository = new TaskRepository();
            CurrentTask = selectedTask;
            UpdateTaskCommand = new DelegateCommand(Update);
        }

        public Task CurrentTask { get; set; }
        public ICommand UpdateTaskCommand { get; }

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
    }
}
