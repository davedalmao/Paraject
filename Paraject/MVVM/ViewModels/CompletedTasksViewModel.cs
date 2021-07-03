using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class CompletedTasksViewModel : BaseViewModel
    {
        private readonly TaskRepository _taskRepository;
        private readonly TasksViewModel _tasksViewModel;
        private readonly int _projectId;

        public CompletedTasksViewModel(TasksViewModel tasksViewModel, int projectId)
        {
            _taskRepository = new TaskRepository();

            _tasksViewModel = tasksViewModel;
            _projectId = projectId;

            FilterTasksCommand = new DelegateCommand(FilterTasks);
            NavigateToTaskDetailsViewCommand = new ParameterizedDelegateCommand(NavigateToTaskDetailsView);

            DisplayAllFilteredTasks();
        }

        #region Properties
        public ObservableCollection<Task> CompletedTasks { get; set; }
        public ObservableCollection<GridTileData> CardTasksGrid { get; set; }

        public string CurrentTaskType { get; set; } = "Show All";
        public string CategoryFilter { get; set; } = "Show All";

        public ICommand FilterTasksCommand { get; }
        public ICommand NavigateToTaskDetailsViewCommand { get; }
        #endregion

        #region Methods
        private void SetValuesForTasksCollection()
        {
            CompletedTasks = null;
            CompletedTasks = new ObservableCollection<Task>(_taskRepository.FindAll(_projectId, CurrentTaskType, "Completed", null, CategoryFilter));
        }
        private void SetNewGridDisplay()
        {
            CardTasksGrid = null;
            CardTasksGrid = new ObservableCollection<GridTileData>();
        }
        private void FilterTasks()
        {
            SetValuesForTasksCollection();
            SetNewGridDisplay();
            TaskCardGridLocation();
        }
        private void TaskCardGridLocation()
        {
            int row = -1;
            int column = -1;

            //This is for a 3 column grid, with n number of rows
            for (int i = 0; i < CompletedTasks.Count; i++)
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

                GridTileData td = new(CompletedTasks[i], row, column);
                CardTasksGrid.Add(td);
            }
        }
        public void DisplayAllFilteredTasks()
        {
            SetValuesForTasksCollection();
            SetNewGridDisplay();
            TaskCardGridLocation();
        }
        public void NavigateToTaskDetailsView(object taskId) //the argument passed to this parameter is in CompletedTasksView (a "CommandParameter" from a Task card)
        {
            Task selectedTask = _taskRepository.Get((int)taskId);
            TaskDetailsViewModel taskDetailsViewModel = new TaskDetailsViewModel(this, _tasksViewModel, selectedTask);

            MainWindowViewModel.CurrentView = taskDetailsViewModel;
        }
        #endregion
    }
}
