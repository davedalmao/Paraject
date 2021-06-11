using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        private readonly ProjectRepository _projectRepository;

        public ICommand AddProjectCommand { get; }
        public ICommand AllProjectsCommand { get; }
        public ICommand PersonalProjectsCommand { get; }
        public ICommand PaidProjectsCommand { get; }
        public ICommand AddProjectsDialogCommand { get; }

        public string TestMessage { get; set; } //test property
        public Project CurrentProject { get; set; }
        public UserAccount CurrentUserAccount { get; set; }

        public ProjectsViewModel(UserAccount userAccount)
        {
            //Repository
            _projectRepository = new ProjectRepository();

            //Models
            CurrentProject = new Project();
            CurrentUserAccount = userAccount;

            //Project Displays in ProjectsView
            AllProjectsCommand = new DelegateCommand(AllProjects);
            PersonalProjectsCommand = new DelegateCommand(PersonalProjects);
            PaidProjectsCommand = new DelegateCommand(PaidProjects);

            //Commands in the AddProjectsModalDialog
            AddProjectsDialogCommand = new DelegateCommand(ShowAddProjectsDialog);
            AddProjectCommand = new DelegateCommand(Add);

            //Default Project Display
            AllProjects();
        }

        public void Add()
        {
            //Validate if a Project has a name
            if (!string.IsNullOrWhiteSpace(CurrentProject.Name))
            {
                bool isAdded = _projectRepository.Add(CurrentProject, CurrentUserAccount.Id);
                AddValidatedProjectToDB(isAdded);
            }

            else
            {
                MessageBox.Show("A project should have a name");
            }
        }

        private static void AddValidatedProjectToDB(bool isAdded)
        {
            if (isAdded)
            {
                MessageBox.Show("Project Created");
            }

            else
            {
                MessageBox.Show("Error occured, cannot create project");
            }
        }


        #region Display Project/s Methods
        public void AllProjects()
        {
            TestMessage = "all";
        }
        public void PersonalProjects()
        {
            TestMessage = "personal";
        }
        public void PaidProjects()
        {
            TestMessage = "paid";
        }
        #endregion

        #region AddProjectModalDialog dialog
        public void ShowAddProjectsDialog()
        {
            //Show overlay from MainWindow
            MainWindowViewModel.Overlay = true;

            AddProjectModalDialog addProjectModalDialog = new AddProjectModalDialog();
            addProjectModalDialog.DataContext = this;
            addProjectModalDialog.ShowDialog();
        }
        #endregion
    }
}
