using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        public ICommand AddProjectCommand { get; }
        public ICommand AllProjectsCommand { get; }
        public ICommand PersonalProjectsCommand { get; }
        public ICommand PaidProjectsCommand { get; }
        public ICommand AddProjectsDialogCommand { get; }

        public string TestMessage { get; set; } //test property
        public Project CurrentProject { get; set; }
        public UserAccount CurrentUserAccount { get; set; }
        //public DisplayProjectsViewModel DisplayProjectsVM { get; set; }

        public ProjectsViewModel(UserAccount userAccount)
        {
            //DisplayProjectsVM = new DisplayProjectsViewModel();
            CurrentProject = new Project();
            CurrentUserAccount = userAccount;

            AddProjectCommand = new DelegateCommand(Add);
            AllProjectsCommand = new DelegateCommand(AllProjects);
            PersonalProjectsCommand = new DelegateCommand(PersonalProjects);
            PaidProjectsCommand = new DelegateCommand(PaidProjects);
            AddProjectsDialogCommand = new DelegateCommand(ShowAddProjectsDialog);

            AllProjects();
        }

        public void Add()
        {
            MessageBox.Show($"User Id Fk: {CurrentUserAccount.Id} \nName: {CurrentProject.Name} \nDecription: {CurrentProject.Description} \nOption: {CurrentProject.Option} \nDeadline: {CurrentProject.Deadline} \nDate Created: {DateTime.Now}");
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
