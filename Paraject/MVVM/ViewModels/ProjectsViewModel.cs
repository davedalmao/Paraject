using Microsoft.Win32;
using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        private readonly ProjectRepository _projectRepository;
        public event EventHandler Closed; //The Window (AddProjectModalDialog) closes itself when this event is executed

        public ICommand AddProjectCommand { get; }
        public ICommand AddProjectLogoCommand { get; }
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
            AddProjectLogoCommand = new DelegateCommand(LoadProjectLogo);

            //Default Project Display
            AllProjects();
        }


        #region Add Project Methods
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
        private void AddValidatedProjectToDB(bool isAdded)
        {
            if (isAdded)
            {
                MessageBox.Show("Project Created");
                MainWindowViewModel.Overlay = false;

                //close the AddProjectModalDialog if  Creating a Project is successful
                Close();
            }

            else
            {
                MessageBox.Show("Error occured, cannot create project");
            }
        }
        #endregion

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
        private void LoadProjectLogo()
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Title = "Select the project's logo",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };

            if (openFile.ShowDialog() == true)
            {
                CurrentProject.Logo = Image.FromFile(openFile.FileName);
            }
        }
        private void Close() //The method that executes Closed EventHandler
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
