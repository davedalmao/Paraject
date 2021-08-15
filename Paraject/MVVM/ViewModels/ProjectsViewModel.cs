using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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

            ShowAddProjectsDialogCommand = new ParameterizedDelegateCommand(ShowAddProjectModalDialog);

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

        public bool AddNewProjectButtonIsVisible { get; set; } = true;
        public bool ProjectOptionComboBoxIsVisible { get; set; }
        public string CurrentProjectOption { get; set; }

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
            ShowAddNewProjectButton();
            Projects = new ObservableCollection<Project>(_projectRepository.GetAll(_currentUserId));
        }
        public void DisplayPersonalProjects()
        {
            ShowAddNewProjectButton();
            Projects = new ObservableCollection<Project>(_projectRepository.FindAll(_currentUserId, ProjectOptions.Personal)
                                                                           .Where(project => project.Status != "Completed"));
        }
        public void DisplayPaidProjects()
        {
            ShowAddNewProjectButton();
            Projects = new ObservableCollection<Project>(_projectRepository.FindAll(_currentUserId, ProjectOptions.Paid)
                                                                           .Where(project => project.Status != "Completed"));
        }
        private void DisplayCompletedProjects()
        {
            if (CompletedButtonIsChecked)
            {
                ShowProjectOptionComboBox();
                GetValuesForProjectsCollection();
            }
        }
        private void GetValuesForProjectsCollection()
        {
            if (CurrentProjectOption == "Show All")
            {
                Projects = new ObservableCollection<Project>(_projectRepository.GetAll(_currentUserId).Where(project => project.Status == "Completed"));
                return;
            }

            Projects = new ObservableCollection<Project>(_projectRepository.GetAll(_currentUserId)
                                                                           .Where(project => project.Option == CurrentProjectOption && project.Status == "Completed"));
        }
        public void RefreshProjects()
        {
            if (PersonalButtonIsChecked)
            {
                DisplayPersonalProjects();
            }

            else if (PaidButtonIsChecked)
            {
                DisplayPaidProjects();
            }

            else if (CompletedButtonIsChecked)
            {
                DisplayCompletedProjects();
            }
            else
            {
                DisplayAllProjects();
            }
        }

        private void ShowAddNewProjectButton()
        {
            if (!AddNewProjectButtonIsVisible)
            {
                AddNewProjectButtonIsVisible = true;
                ProjectOptionComboBoxIsVisible = false;
            }
        }
        private void ShowProjectOptionComboBox()
        {
            if (!ProjectOptionComboBoxIsVisible && CompletedButtonIsChecked)
            {
                CurrentProjectOption = "Show All"; //To set Project Option ComboBox's default value EVERYTIME CompletedButtonIsChecked

                ProjectOptionComboBoxIsVisible = true;
                AddNewProjectButtonIsVisible = false;
            }
        }

        public void ShowAddProjectModalDialog(object owner)
        {
            MainWindowViewModel.Overlay = true;

            AddProjectModalDialog addProjectModalDialog = new()
            {
                DataContext = new AddProjectModalDialogViewModel(RefreshProjects, _currentUserId),
                Owner = owner as Window
            };
            addProjectModalDialog.ShowDialog();
        }
        public void NavigateToTasksView(object projectId) //the argument passed to this parameter is in ProjectsView (a "CommandParameter" from a Project card)
        {
            Project selectedProject = _projectRepository.Get((int)projectId);

            ProjectContentViewModel tasksViewModel = new ProjectContentViewModel(this, RefreshProjects, selectedProject);
            MainWindowViewModel.CurrentView = tasksViewModel;
        }
        #endregion
    }
}
