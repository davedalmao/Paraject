using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class SubtasksViewModel : BaseViewModel
    {
        private readonly TaskDetailsViewModel _taskDetailsViewModel;

        public SubtasksViewModel(TaskDetailsViewModel taskDetailsViewModel)
        {
            _taskDetailsViewModel = taskDetailsViewModel;
            NavigateBackToTaskDetailsViewCommand = new DelegateCommand(NavigateBackToTaskDetailsView);
            FilterTasksCommand = new DelegateCommand(DisplayAllFilteredTasks);
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
