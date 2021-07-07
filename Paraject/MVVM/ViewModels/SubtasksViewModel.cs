using Paraject.Core.Commands;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;
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

        public string StatusFilter { get; set; } = "Show All";
        public string PriorityFilter { get; set; } = "Show All";
        public ICommand NavigateBackToTaskDetailsViewCommand { get; }
        public ICommand FilterTasksCommand { get; }


        private void NavigateBackToTaskDetailsView()
        {
            MainWindowViewModel.CurrentView = _taskDetailsViewModel;
        }

        private void DisplayAllFilteredTasks()
        {
            MessageBox.Show($"Status: {StatusFilter} \nPriority: {PriorityFilter}");
        }
    }
}
