using Paraject.MVVM.Models;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectDetailsViewModel : BaseViewModel
    {
        public ProjectDetailsViewModel(Project currentProject)
        {
            CurrentProject = currentProject;
        }

        public Project CurrentProject { get; set; }
    }
}
