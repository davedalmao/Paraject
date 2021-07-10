using Paraject.MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class AllSubtasksViewModel : BaseViewModel
    {
        public AllSubtasksViewModel(bool isCompletedButtonChecked)
        {
            InputRowVisibility = isCompletedButtonChecked;
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

    }
}
