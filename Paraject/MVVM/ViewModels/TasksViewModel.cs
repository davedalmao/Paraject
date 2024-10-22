﻿using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private readonly TaskRepository _taskRepository;
        private readonly ProjectContentViewModel _tasksViewModel;
        private readonly string _currentTaskType;

        public TasksViewModel(ProjectContentViewModel tasksViewModel, Project parentProject, string currentTaskType = null)
        {
            _taskRepository = new TaskRepository();

            _currentTaskType = currentTaskType;
            _tasksViewModel = tasksViewModel;
            ParentProject = parentProject;

            ShowAddTaskModalDialogCommand = new RelayCommand(ShowAddTaskModalDialog);
            FilterTasksCommand = new RelayCommand(DisplayAllFilteredTasks);
            NavigateToSubtasksViewCommand = new RelayCommand<object>(NavigateToSubtasksView);

            DisplayAllFilteredTasks();
        }

        #region Properties
        public Project ParentProject { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<GridTileData> CardTasksGrid { get; set; }

        //Default Inputs' Values
        public bool TaskTypeComboBoxIsVisible { get; set; }
        public bool TaskStatusComboBoxIsVisible { get; set; } = true;
        public bool TaskPriorityComboBoxIsVisible { get; set; } = true;
        public bool TaskCategoryComboBoxIsVisible { get; set; } = true;
        public bool AddNewTaskButtonIsVisible { get; set; } = true;

        //Default ComboBoxs' Values
        public string StatusFilter { get; set; } = "Show All";
        public string PriorityFilter { get; set; } = "Show All";
        public string CategoryFilter { get; set; } = "Show All";
        public string CurrentTaskType { get; set; } = "Show All";

        public ICommand ShowAddTaskModalDialogCommand { get; }
        public ICommand FilterTasksCommand { get; }
        public ICommand NavigateToSubtasksViewCommand { get; }
        #endregion

        #region Methods
        public void DisplayAllFilteredTasks()
        {
            SetValuesForTasksCollection();
            SetNewGridDisplay();
            TaskCardGridLocation();
        }
        private void SetValuesForTasksCollection()
        {
            Tasks = null;
            InputsToDisplay();

            if (_currentTaskType is null)
            {
                Tasks = new ObservableCollection<Task>(_taskRepository.FindAll(ParentProject.Id, CurrentTaskType, "Completed", null, CategoryFilter));
                return;
            }

            Tasks = new ObservableCollection<Task>(_taskRepository.FindAll(ParentProject.Id, _currentTaskType, StatusFilter, PriorityFilter, CategoryFilter)
                                                                  .Where(task => task.Status != "Completed"));
        }
        private void InputsToDisplay()
        {
            //For "Completed" status tasks
            if (_currentTaskType is null)
            {
                TaskTypeComboBoxIsVisible = true;
                TaskCategoryComboBoxIsVisible = true;

                TaskStatusComboBoxIsVisible = false;
                TaskPriorityComboBoxIsVisible = false;
                AddNewTaskButtonIsVisible = false;

                return;
            }

            //For "In Progress" and "Open" status tasks
            TaskTypeComboBoxIsVisible = false;
            TaskStatusComboBoxIsVisible = true;
            TaskPriorityComboBoxIsVisible = true;
            TaskCategoryComboBoxIsVisible = true;
            AddNewTaskButtonIsVisible = true;
        }
        private void SetNewGridDisplay()
        {
            CardTasksGrid = null;
            CardTasksGrid = new ObservableCollection<GridTileData>();
        }
        private void TaskCardGridLocation()
        {
            int row = -1;
            int column = -1;

            //This is for a 3 column grid, with n number of rows
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

        private void ShowAddTaskModalDialog()
        {
            MainWindowViewModel.Overlay = true;

            AddTaskModalDialog addTaskModalDialog = new()
            {
                DataContext = new AddTaskModalDialogViewModel(DisplayAllFilteredTasks, ParentProject, _currentTaskType),
                Owner = GetMainWindow.MainWindowObject
            };
            addTaskModalDialog.ShowDialog();
        }
        public void NavigateToSubtasksView(object taskId) //the argument passed to this parameter is in ProjectsView (a "CommandParameter" from a Project card)
        {
            Task selectedTask = _taskRepository.Get((int)taskId);
            TaskContentViewModel subtasksViewModel = new(DisplayAllFilteredTasks, _tasksViewModel, selectedTask, ParentProject);

            MainWindowViewModel.CurrentView = subtasksViewModel;
        }
        #endregion
    }
}
