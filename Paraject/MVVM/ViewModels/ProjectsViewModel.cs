using Paraject.Core.Commands;
using Paraject.MVVM.Views.ModalDialogs;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        public ICommand AllProjectsCommand { get; }
        public ICommand PersonalProjectsCommand { get; }
        public ICommand PaidProjectsCommand { get; }
        public ICommand AddProjectsDialogCommand { get; }

        public string Message { get; set; } //test property

        public DisplayProjectsViewModel DisplayProjectsVM { get; set; }

        public ProjectsViewModel()
        {
            DisplayProjectsVM = new DisplayProjectsViewModel();

            AllProjectsCommand = new DelegateCommand(AllProjects);
            PersonalProjectsCommand = new DelegateCommand(PersonalProjects);
            PaidProjectsCommand = new DelegateCommand(PaidProjects);
            AddProjectsDialogCommand = new DelegateCommand(ShowAddProjectsDialog);

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
        public void ShowAddProjectsDialog()
        {
            AddProjectModalDialog addProjectModalDialog = new AddProjectModalDialog();
            addProjectModalDialog.ShowDialog();
        }
    }
}
