using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private readonly UserAccountRepository _userAccountRepository;

        public TasksViewModel(ProjectsViewModel projectsViewModel, Project currentProject)
        {
            CurrentProject = currentProject;
            _userAccountRepository = new UserAccountRepository();

            //TasksView child Views
            TasksTodoVMFinishLine = new TasksTodoViewModel(currentProject.Id, TaskTypes.FinishLine);
            TasksTodoVMExtraFeatures = new TasksTodoViewModel(currentProject.Id, TaskTypes.ExtraFeature);
            CompletedTasksVM = new CompletedTasksViewModel();
            ProjectNotesVM = new ProjectNotesViewModel();
            ProjectDetailsVM = new ProjectDetailsViewModel(projectsViewModel, currentProject);

            CurrentView = TasksTodoVMFinishLine;

            //TasksView child Views
            FinishLineTasksCommand = new ParameterizedDelegateCommand(o => { CurrentView = TasksTodoVMFinishLine; });
            ExtraFeaturesTasksCommand = new ParameterizedDelegateCommand(o => { CurrentView = TasksTodoVMExtraFeatures; });
            CompletedTasksViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = CompletedTasksVM; });
            ProjectNotesViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = ProjectNotesVM; });
            ProjectDetailsViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = ProjectDetailsVM; });
            NavigateBackToProjectsViewCommand = new DelegateCommand(NavigateBackToProjectsView);
        }

        #region Properties
        public Project CurrentProject { get; set; }
        public object CurrentView { get; set; }
        #endregion

        #region ViewModels (that will navigate with their associated Views)
        public ProjectsViewModel ProjectsVM { get; set; }

        //TasksView child Views
        public TasksTodoViewModel TasksTodoVMFinishLine { get; set; }
        public TasksTodoViewModel TasksTodoVMExtraFeatures { get; set; }
        public CompletedTasksViewModel CompletedTasksVM { get; set; }
        public ProjectNotesViewModel ProjectNotesVM { get; set; }
        public ProjectDetailsViewModel ProjectDetailsVM { get; set; }
        #endregion

        #region Commands
        public ICommand NavigateBackToProjectsViewCommand { get; }
        public ICommand FinishLineTasksCommand { get; }
        public ICommand ExtraFeaturesTasksCommand { get; }
        public ICommand CompletedTasksViewCommand { get; }
        public ICommand ProjectNotesViewCommand { get; }
        public ICommand ProjectDetailsViewCommand { get; }
        #endregion

        #region Navigation Methods
        public void NavigateBackToProjectsView()
        {
            ProjectsViewModel projectsViewModel = new ProjectsViewModel(_userAccountRepository.GetById(CurrentProject.User_Id_Fk));
            ProjectsVM = projectsViewModel;
            MainWindowViewModel.CurrentView = ProjectsVM;
        }
        #endregion

    }
}