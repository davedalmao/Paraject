using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class SubtasksViewModel : BaseViewModel
    {
        private readonly TaskDetailsViewModel _taskDetailsViewModel;
        private readonly SubtaskRepository _subtaskRepository;
        private readonly int _taskId;

        public SubtasksViewModel(TaskDetailsViewModel taskDetailsViewModel, int taskId)
        {
            _subtaskRepository = new SubtaskRepository();
            _taskDetailsViewModel = taskDetailsViewModel;
            _taskId = taskId;

            NavigateBackToTaskDetailsViewCommand = new DelegateCommand(NavigateBackToTaskDetailsView);
            FilterTasksCommand = new DelegateCommand(DisplayAllFilteredTasks);

            DisplayAllFilteredTasks();
        }
        public ObservableCollection<Subtask> Subtasks { get; set; }
        public ObservableCollection<GridTileData> CardSubtasksGrid { get; set; }

        public string StatusFilter { get; set; } = "Show All";
        public string PriorityFilter { get; set; } = "Show All";

        public ICommand NavigateBackToTaskDetailsViewCommand { get; }
        public ICommand FilterTasksCommand { get; }


        private void NavigateBackToTaskDetailsView()
        {
            MainWindowViewModel.CurrentView = _taskDetailsViewModel;
        }

        private void GetValuesForSubtasksCollection()
        {
            Subtasks = null;
            Subtasks = new ObservableCollection<Subtask>(_subtaskRepository.FindAll(_taskId, StatusFilter, PriorityFilter)
                                                                           .Where(subtask => subtask.Status != "Completed"));
            //Subtasks = new ObservableCollection<Task>(_taskRepository.FindAll(_projectId, _currentTaskType, StatusFilter, PriorityFilter, CategoryFilter)
            //                                                      .Where(task => task.Status != "Completed"));
        }
        private void SetNewGridDisplay()
        {
            CardSubtasksGrid = null;
            CardSubtasksGrid = new ObservableCollection<GridTileData>();
        }
        private void SubtaskCardGridLocation()
        {
            int row = -1;
            int column = -1;

            //This is for a 3 column grid, with n number of rows
            for (int i = 0; i < Subtasks.Count; i++)
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

                GridTileData td = new(Subtasks[i], row, column);
                CardSubtasksGrid.Add(td);
            }
        }
        private void DisplayAllFilteredTasks()
        {
            //MessageBox.Show($"Status: {StatusFilter} \nPriority: {PriorityFilter}");
            GetValuesForSubtasksCollection();
            SetNewGridDisplay();
            SubtaskCardGridLocation();
        }
    }
}
