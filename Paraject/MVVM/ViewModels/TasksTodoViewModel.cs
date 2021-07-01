using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TasksTodoViewModel : BaseViewModel
    {
        private readonly int _projectId;
        private readonly TaskRepository _taskRepository;
        private readonly string _currentTaskType;

        public TasksTodoViewModel(int projectId, string taskType)
        {
            _taskRepository = new TaskRepository();
            _projectId = projectId;
            _currentTaskType = taskType;

            ShowAddTaskModalDialogCommand = new DelegateCommand(ShowAddTaskModalDialog);
            FilterTasksCommand = new DelegateCommand(DisplayAllFilteredTasks);

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
        #endregion

        #region Methods
        private void ShowAddTaskModalDialog()
        {
            MainWindowViewModel.Overlay = true;

            AddTaskModalDialogViewModel addTaskModalDialogViewModel = new AddTaskModalDialogViewModel(_projectId, _currentTaskType);

            AddTaskModalDialog addTaskModalDialog = new();
            addTaskModalDialog.DataContext = addTaskModalDialogViewModel;
            addTaskModalDialog.ShowDialog();
        }
        private void SetValuesForTasksCollection()
        {
            Tasks = null;
            Tasks = new ObservableCollection<Task>(_taskRepository.FindAll(_projectId, _currentTaskType, StatusFilter, PriorityFilter, CategoryFilter));
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
        private void DisplayAllFilteredTasks()
        {
            SetValuesForTasksCollection();
            SetNewGridDisplay();
            TaskCardGridLocation();
        }
        #endregion
    }
}
