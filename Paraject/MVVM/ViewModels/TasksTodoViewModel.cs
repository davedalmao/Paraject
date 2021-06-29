using Paraject.Core.Commands;
using Paraject.Core.Enums;
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

        public TasksTodoViewModel(int projectId, TaskTypes taskTypes)
        {
            _projectId = projectId;
            CurrentTaskType = taskTypes;

            _taskRepository = new TaskRepository();

            CurrentTask = new Task();
            CardTasksGrid = new ObservableCollection<GridTileData>();

            ShowAddTaskModalDialogCommand = new DelegateCommand(ShowAddTaskModalDialog);
            CloseModalCommand = new DelegateCommand(CloseModal);
            AddTaskCommand = new DelegateCommand(Add);
            FilterTasksCommand = new DelegateCommand(FilterTasks);

            //default tasks display
            Tasks = new ObservableCollection<Task>(_taskRepository.FindAll(_projectId, CurrentTaskType, StatusFilter, PriorityFilter, CategoryFilter));
            CardGridLocations();
        }

        #region Properties
        //TaskType (FinishLine or ExtraFeature)
        public TaskTypes CurrentTaskType { get; set; }

        //Collections
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<GridTileData> CardTasksGrid { get; set; }

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
        private void CardGridLocations()
        {
            _row = -1;
            _column = -1;

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

                GridTileData td = new(Tasks[i], _row, _column);
                CardTasksGrid.Add(td);
            }
        }
        private void ShowAddTaskModalDialog()
        {
            //Show overlay from MainWindow
            MainWindowViewModel.Overlay = true;

            AddTaskModalDialog addTaskModalDialog = new();
            CurrentTask.Type = Enum.GetName(CurrentTaskType);
            addTaskModalDialog.DataContext = this;
            addTaskModalDialog.ShowDialog();
        }
        private void CloseModal()
        {
            MainWindowViewModel.Overlay = false;

            SetTaskDefault();

            foreach (Window currentModal in Application.Current.Windows)
            {
                if (currentModal.DataContext == this)
                {
                    currentModal.Close();
                }
            }
        }
        private void SetTaskDefault()
        {
            //To erase the last input values in AddProjectModalDialog, and Display the newly added Task after a successful add operation
            CurrentTask = null;
            Tasks = null;
            CardTasksGrid = null;

            CurrentTask = new Task();
            Tasks = new ObservableCollection<Task>(_taskRepository.FindAll(_projectId, CurrentTaskType, StatusFilter, PriorityFilter, CategoryFilter));
            CardTasksGrid = new ObservableCollection<GridTileData>();

            CardGridLocations();
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
                CloseModal();
            }

            else
            {
                MessageBox.Show("Error occured, cannot create task");
            }
        }
        private void FilterTasks()
        {
            //try
            //{
            //    //MessageBox.Show($"Status: {StatusFilter}, Priority: {PriorityFilter}, Category: {CategoryFilter}");
            //    FinishLineTasks();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
        #endregion
    }
}
