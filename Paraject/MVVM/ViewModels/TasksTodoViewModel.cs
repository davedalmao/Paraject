using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TasksTodoViewModel : BaseViewModel
    {
        private readonly int _projectId;
        private readonly TaskRepository _taskRepository;

        public TasksTodoViewModel(int projectId)
        {
            _projectId = projectId;
            _taskRepository = new TaskRepository();
            CurrentTask = new Task();

            ShowAddTaskModalDialogCommand = new DelegateCommand(ShowAddTaskModalDialog);
            CloseModalCommand = new DelegateCommand(SetTaskDefaultThenCloseModal);
            AddTaskCommand = new DelegateCommand(Add);
            FilterTasksCommand = new DelegateCommand(FilterTasks);
        }

        #region Properties
        //ComboBox (default) Filter Bindings
        public string StatusFilter { get; set; } = "Show All";
        public string PriorityFilter { get; set; } = "Show All";
        public string CategoryFilter { get; set; } = "Show All";

        //Model
        public Task CurrentTask { get; set; }

        //Commands
        public ICommand ShowAddTaskModalDialogCommand { get; }
        public ICommand CloseModalCommand { get; }
        public ICommand AddTaskCommand { get; }
        public ICommand FilterTasksCommand { get; }
        #endregion

        #region Methods
        private void FilterTasks()
        {
            MessageBox.Show($"Status: {StatusFilter}, Priority: {PriorityFilter}, Category: {CategoryFilter}");
        }
        private void ShowAddTaskModalDialog()
        {
            //Show overlay from MainWindow
            MainWindowViewModel.Overlay = true;

            AddTaskModalDialog addTaskModalDialog = new();
            addTaskModalDialog.DataContext = this;
            addTaskModalDialog.ShowDialog();
        }
        private void SetTaskDefaultThenCloseModal()
        {
            MainWindowViewModel.Overlay = false;

            //To erase the last input values in AddProjectModalDialog
            CurrentTask = null;
            CurrentTask = new Task();

            foreach (Window currentModal in Application.Current.Windows)
            {
                if (currentModal.DataContext == this)
                {
                    currentModal.Close();
                }
            }
        }
        private void Add()
        {
            if (!string.IsNullOrWhiteSpace(CurrentTask.Subject))
            {
                bool isAdded = _taskRepository.Add(CurrentTask, _projectId);
                CheckTaskCreation(isAdded);
            }

            else
            {
                MessageBox.Show("A task should have a subject");
            }
        }
        private void CheckTaskCreation(bool isAdded)
        {
            if (isAdded)
            {
                MessageBox.Show("Task Created");
                SetTaskDefaultThenCloseModal();
            }

            else
            {
                MessageBox.Show("Error occured, cannot create task");
            }
        }
        #endregion
    }
}
