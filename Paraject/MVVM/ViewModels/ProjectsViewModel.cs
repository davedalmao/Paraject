using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        private readonly ProjectRepository _projectRepository;
        private readonly int _currentUserId;

        public ProjectsViewModel(int currentUserId)
        {
            _currentUserId = currentUserId;
            _projectRepository = new ProjectRepository();

            AllProjectsCommand = new DelegateCommand(DisplayAllProjects);
            PersonalProjectsCommand = new DelegateCommand(DisplayPersonalProjects);
            PaidProjectsCommand = new DelegateCommand(DisplayPaidProjects);
            CompletedProjectsCommand = new DelegateCommand(DisplayCompletedProjects);

            ShowAddProjectsDialogCommand = new DelegateCommand(ShowAddProjectModalDialog);

            TasksViewCommand = new ParameterizedDelegateCommand(NavigateToTasksView); //Redirect to TasksView if a Project card is selected (to view a Project's task/s)
            DisplayAllProjects();
        }

        #region Properties
        public ObservableCollection<Project> Projects { get; set; }

        //RadioButtons in ProjectsView
        public bool AllProjectsButtonIsChecked { get; set; } = true; //default selected RadioButton
        public bool PersonalButtonIsChecked { get; set; }
        public bool PaidButtonIsChecked { get; set; }
        public bool CompletedButtonIsChecked { get; set; }

        public ICommand AllProjectsCommand { get; }
        public ICommand PersonalProjectsCommand { get; }
        public ICommand PaidProjectsCommand { get; }
        public ICommand CompletedProjectsCommand { get; }
        public ICommand ShowAddProjectsDialogCommand { get; }
        public ICommand TasksViewCommand { get; }
        #endregion

        #region Methods
        public void DisplayAllProjects()
        {
            Projects = new ObservableCollection<Project>(_projectRepository.GetAll(_currentUserId));
        }
        public void DisplayPersonalProjects()
        {
            Projects = new ObservableCollection<Project>(_projectRepository.FindAll(_currentUserId, ProjectOptions.Personal));
        }
        public void DisplayPaidProjects()
        {
            Projects = new ObservableCollection<Project>(_projectRepository.FindAll(_currentUserId, ProjectOptions.Paid));
        }
        private void DisplayCompletedProjects()
        {
            Projects = new ObservableCollection<Project>(_projectRepository.GetAll(_currentUserId).Where(project => project.Status == "Completed"));
        }
        public void ShowAddProjectModalDialog()
        {
            MainWindowViewModel.Overlay = true;

            AddProjectModalDialogViewModel addProjectModalDialogViewModel = new AddProjectModalDialogViewModel(_currentUserId);

            AddProjectModalDialog addProjectModalDialog = new AddProjectModalDialog();
            addProjectModalDialog.DataContext = addProjectModalDialogViewModel;
            addProjectModalDialog.ShowDialog();
        }
        public void NavigateToTasksView(object projectId) //the argument passed to this parameter is in ProjectsView (a "CommandParameter" from a Project card)
        {
            Project selectedProject = _projectRepository.Get((int)projectId);

            TasksViewModel tasksViewModel = new TasksViewModel(this, selectedProject);
            MainWindowViewModel.CurrentView = tasksViewModel;
        }
        #endregion
    }
}
