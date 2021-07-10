using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class AllSubtasksViewModel : BaseViewModel
    {
        private readonly SubtaskRepository _subtaskRepository;
        private readonly int _taskId;

        public AllSubtasksViewModel(string filterType, bool isCompletedButtonChecked, int taskId)
        {
            _subtaskRepository = new SubtaskRepository();
            _taskId = taskId;
            InputRowVisibility = isCompletedButtonChecked;

            DisplayAllFilteredSubtasks(filterType);
        }

        #region Properties
        public ObservableCollection<Subtask> Subtasks { get; set; }
        public ObservableCollection<GridTileData> CardSubtasksGrid { get; set; }

        public string StatusFilter { get; set; } = "Show All";
        public string PriorityFilter { get; set; } = "Show All";

        public bool InputRowVisibility { get; set; }

        public ICommand FilterSubtasksCommand { get; }
        public ICommand ShowSubtaskDetailsModalDialogCommand { get; }
        public ICommand ShowAddSubtaskModalDialogCommand { get; }
        #endregion

        #region Methods
        private void DisplaySubtasksTodo()
        {
            Subtasks = new ObservableCollection<Subtask>(_subtaskRepository.FindAll(_taskId, StatusFilter, PriorityFilter)
                                                                           .Where(subtask => subtask.Status != "Completed"));
            //if (!ComboBoxesRowVisibility)
            //{
            //    ComboBoxesRowVisibility = true;

            //    //Just Comment this out if you want to remember the last selected items in the ComboBoxes
            //    StatusFilter = "Show All";
            //    PriorityFilter = "Show All";
            //}

            SubtaskCardGridDisplayAndLocation();
        }
        private void DisplayCompletedSubtasks()
        {
            Subtasks = new ObservableCollection<Subtask>(_subtaskRepository.GetAll(_taskId)
                                                                           .Where(subtask => subtask.Status == "Completed"));

            //if (CompletedSubtasksIsChecked) { ComboBoxesRowVisibility = false; }

            SubtaskCardGridDisplayAndLocation();
        }

        private void DisplayAllFilteredSubtasks(string filterType)
        {
            //DisplaySubtasksTodo();
            //SubtaskCardGridDisplayAndLocation();

            if (filterType == "SubtasksTodo") //this is a CommandParameter in SubtasksView
            {
                DisplaySubtasksTodo();
                return;
            }

            DisplayCompletedSubtasks();
        }

        private void SubtaskCardGridDisplayAndLocation()
        {
            SetNewGridDisplay();
            SubtaskCardGridLocation();
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
        #endregion
    }
}
