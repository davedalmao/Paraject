using Paraject.Core.Commands;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        public ICommand AllProjectsCommand { get; }
        public ICommand PersonalProjectsCommand { get; }
        public ICommand PaidProjectsCommand { get; }

        public string Message { get; set; } //test property

        public DisplayProjectsViewModel DisplayProjectsVM { get; set; }

        public ProjectsViewModel()
        {
            DisplayProjectsVM = new DisplayProjectsViewModel();

            AllProjectsCommand = new DelegateCommand(AllProjects);
            PersonalProjectsCommand = new DelegateCommand(PersonalProjects);
            PaidProjectsCommand = new DelegateCommand(PaidProjects);

            AllProjects();
        }

        public void AllProjects()
        {
            Message = "all";
        }
        public void PersonalProjects()
        {
            Message = "personal";
        }
        public void PaidProjects()
        {
            Message = "paid";
        }
    }
}
