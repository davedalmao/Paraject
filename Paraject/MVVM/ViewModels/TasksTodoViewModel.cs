using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TasksTodoViewModel : BaseViewModel
    {
        private readonly int _projectId;
        private readonly TaskRepository _taskRepository;
        private int _row;
        private int _column;
        public TasksTodoViewModel(int projectId)
        {
            _projectId = projectId;
            _taskRepository = new TaskRepository();

            _row = -1;
            _column = -1;

            CurrentTask = new Task();
            GridTasks = new ObservableCollection<GridTileData>();

            ShowAddTaskModalDialogCommand = new DelegateCommand(ShowAddTaskModalDialog);
            CloseModalCommand = new DelegateCommand(SetTaskDefaultThenCloseModal);
            AddTaskCommand = new DelegateCommand(Add);
            FilterTasksCommand = new DelegateCommand(FilterTasks);

            //default tasks display
            FinishLineTasks();
        }

        #region Properties
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<GridTileData> GridTasks { get; set; }

        //ComboBox Filter Bindings
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
        private void FinishLineTasks()
        {
            Tasks = new ObservableCollection<Task>(_taskRepository.FindAll(_projectId, "FinishLine", StatusFilter, PriorityFilter, CategoryFilter));
            CardGridLocations();
        }

        private void CardGridLocations()
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                if (_column == 2)
                {
                    _column = 0;
                }

                else
                {
                    _column++;
                }

                if (i % 3 == 0)
                {
                    _row++;
                }

                GridTileData td = new GridTileData(Tasks[i], _row, _column);

                GridTasks.Add(td);
            }
        }

        private void FilterTasks()
        {
            try
            {
                //MessageBox.Show($"Status: {StatusFilter}, Priority: {PriorityFilter}, Category: {CategoryFilter}");
                FinishLineTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
