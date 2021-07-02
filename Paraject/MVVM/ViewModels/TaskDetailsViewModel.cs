using Paraject.MVVM.Models;

namespace Paraject.MVVM.ViewModels
{
    public class TaskDetailsViewModel : BaseViewModel
    {
        public TaskDetailsViewModel(Task selectedTask)
        {
            CurrentTask = selectedTask;
        }

        public Task CurrentTask { get; set; }
    }
}
