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
    public class TasksTodoViewModel : BaseViewModel
    {
        private readonly int _projectId;
        private readonly TaskRepository _taskRepository;
        private readonly string _currentTaskType;
        private readonly TasksViewModel _tasksViewModel;

        public TasksTodoViewModel(TasksViewModel tasksViewModel, int projectId, string taskType)
        {
            _taskRepository = new TaskRepository();

            _projectId = projectId;
            _currentTaskType = taskType;
            _tasksViewModel = tasksViewModel;

            ShowAddTaskModalDialogCommand = new DelegateCommand(ShowAddTaskModalDialog);
            FilterTasksCommand = new DelegateCommand(DisplayAllFilteredTasks);
            NavigateToTaskDetailsViewCommand = new ParameterizedDelegateCommand(NavigateToTaskDetailsView);

            DisplayAllFilteredTasks();
        }

        #region Properties
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<GridTileData> CardTasksGrid { get; set; }

        public string StatusFilter { get; set; } = "Show All";
        public string PriorityFilter { get; set; } = "Show All";
        public string CategoryFilter { get; set; } = "Show All";

        public ICommand ShowAddTaskModalDialogCommand { get; }
        public ICommand FilterTasksCommand { get; }
        public ICommand NavigateToTaskDetailsViewCommand { get; }
        #endregion

        #region Methods
        private void ShowAddTaskModalDialog()
        {
            MainWindowViewModel.Overlay = true;

            AddTaskModalDialogViewModel addTaskModalDialogViewModel = new AddTaskModalDialogViewModel(DisplayAllFilteredTasks, _projectId, _currentTaskType);

            AddTaskModalDialog addTaskModalDialog = new();
            addTaskModalDialog.DataContext = addTaskModalDialogViewModel;
            addTaskModalDialog.ShowDialog();
        }
        private void GetValuesForTasksCollection()
        {
            Tasks = null;
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
        public void DisplayAllFilteredTasks()
        {
            GetValuesForTasksCollection();
            SetNewGridDisplay();
            TaskCardGridLocation();
        }
        public void NavigateToTaskDetailsView(object taskId) //the argument passed to this parameter is in ProjectsView (a "CommandParameter" from a Project card)
        {
            Task selectedTask = _taskRepository.Get((int)taskId);
            TaskDetailsViewModel taskDetailsViewModel = new TaskDetailsViewModel(DisplayAllFilteredTasks, _tasksViewModel, selectedTask);

            MainWindowViewModel.CurrentView = taskDetailsViewModel;
        }
        #endregion
    }
}
