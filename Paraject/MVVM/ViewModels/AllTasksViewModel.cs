using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class AllTasksViewModel : BaseViewModel
    {
        private readonly int _projectId;
        private readonly TaskRepository _taskRepository;
        private readonly string _currentTaskType;
        private readonly TasksViewModel _tasksViewModel;

        public AllTasksViewModel(TasksViewModel tasksViewModel, int projectId, Project parentProject, string currentTaskType = null)
        {
            _taskRepository = new TaskRepository();

            _projectId = projectId;
            _currentTaskType = currentTaskType;
            _tasksViewModel = tasksViewModel;
            ParentProject = parentProject;

            ShowAddTaskModalDialogCommand = new DelegateCommand(ShowAddTaskModalDialog);
            FilterTasksCommand = new DelegateCommand(DisplayAllFilteredTasks);
            NavigateToSubtasksViewCommand = new ParameterizedDelegateCommand(NavigateToSubtasksView);

            DisplayAllFilteredTasks();
        }

        #region Properties
        public Project ParentProject { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<GridTileData> CardTasksGrid { get; set; }

        public bool TaskTypeComboBoxIsVisible { get; set; }
        public bool TaskStatusComboBoxIsVisible { get; set; } = true;
        public bool TaskPriorityComboBoxIsVisible { get; set; } = true;
        public bool TaskCategoryComboBoxIsVisible { get; set; } = true;
        public bool AddNewTaskButtonIsVisible { get; set; } = true;

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

            if (_currentTaskType is null)
            {
                Tasks = new ObservableCollection<Task>(_taskRepository.FindAll(_projectId, CurrentTaskType, "Completed", null, CategoryFilter));
                return;
            }

            Tasks = new ObservableCollection<Task>(_taskRepository.FindAll(_projectId, _currentTaskType, StatusFilter, PriorityFilter, CategoryFilter)
                                                                  .Where(task => task.Status != "Completed"));
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

            AddTaskModalDialogViewModel addTaskModalDialogViewModel = new(DisplayAllFilteredTasks, ParentProject, _currentTaskType);

            AddTaskModalDialog addTaskModalDialog = new();
            addTaskModalDialog.DataContext = addTaskModalDialogViewModel;
            addTaskModalDialog.ShowDialog();
        }
        public void NavigateToSubtasksView(object taskId) //the argument passed to this parameter is in ProjectsView (a "CommandParameter" from a Project card)
        {
            Task selectedTask = _taskRepository.Get((int)taskId);
            SubtasksViewModel subtasksViewModel = new(DisplayAllFilteredTasks, _tasksViewModel, selectedTask, ParentProject);

            MainWindowViewModel.CurrentView = subtasksViewModel;
        }
        #endregion
    }
}
