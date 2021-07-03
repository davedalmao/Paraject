using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class CompletedTasksViewModel : BaseViewModel
    {
        private readonly TaskRepository _taskRepository;
        private readonly int _projectId;

        public CompletedTasksViewModel(int projectId)
        {
            _taskRepository = new TaskRepository();
            _projectId = projectId;
            FilterTasksCommand = new DelegateCommand(FilterTasks);

            DisplayAllFilteredTasks();
        }

        #region Properties
        public ObservableCollection<Task> CompletedTasks { get; set; }
        public ObservableCollection<GridTileData> CardTasksGrid { get; set; }

        public string CurrentTaskType { get; set; } = "Show All";
        public string CategoryFilter { get; set; } = "Show All";

        public ICommand FilterTasksCommand { get; }
        #endregion

        #region Methods
        #region Reset Methods
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
        #endregion
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
        private void DisplayAllFilteredTasks()
        {
            SetValuesForTasksCollection();
            SetNewGridDisplay();
            TaskCardGridLocation();
        }
        public void NavigateToTaskDetailsView(object taskId) //the argument passed to this parameter is in ProjectsView (a "CommandParameter" from a Project card)
        {
            Task selectedTask = _taskRepository.Get((int)taskId);
            //TaskDetailsViewModel taskDetailsViewModel = new TaskDetailsViewModel(this, _tasksViewModel, selectedTask);

            //MainWindowViewModel.CurrentView = taskDetailsViewModel;
        }
        #endregion
    }
}
