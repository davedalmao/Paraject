namespace Paraject.MVVM.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        public TasksViewModel(int? projectId)
        {
            Test = projectId;
        }

        public int? Test { get; set; }
    }
}
