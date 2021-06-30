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
        #region AddTaskModalDialog Methods
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

            ResetValues();

            foreach (Window currentModal in Application.Current.Windows)
            {
                if (currentModal.DataContext == this)
                {
                    currentModal.Close();
                }
            }
        }
        #endregion

        #region Reset Methods
        private void ResetValues()
        {
            SetCurrentTaskDefaultValues();
            SetValuesForTasksCollection();

            SetNewGridDisplay();
            CardGridLocations();
        }
        private void SetCurrentTaskDefaultValues()
        {
            CurrentTask = null;
            CurrentTask = new Task();
        }
        private void SetValuesForTasksCollection()
        {
            Tasks = null;
            Tasks = new ObservableCollection<Task>(_taskRepository.FindAll(_projectId, CurrentTaskType, StatusFilter, PriorityFilter, CategoryFilter));
        }
        private void SetNewGridDisplay()
        {
            CardTasksGrid = null;
            CardTasksGrid = new ObservableCollection<GridTileData>();
        }
        #endregion

        #region Add Task Methods
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
        #endregion

        private void CardGridLocations()
        {
            int row = -1;
            int column = -1;

            for (int i = 0; i < Tasks.Count; i++)
            {
                if (column == 2)
                {
                    column = 0;
                }

                else
                {
                    column++;
                }

                if (i % 3 == 0)
                {
                    row++;
                }

                GridTileData td = new(Tasks[i], row, column);
                CardTasksGrid.Add(td);
            }
        }
        private void FilterTasks()
        {
            SetValuesForTasksCollection();
            SetNewGridDisplay();
            CardGridLocations();
        }
        #endregion
    }
}
