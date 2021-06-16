namespace Paraject.MVVM.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        public TasksViewModel(int? projectId)
        {
            Test = projectId;
            //currentview = TasksTodoView
        }

        public int? Test { get; set; }

        public object CurrentView { get; set; }
    }
}
