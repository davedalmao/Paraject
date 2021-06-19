using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private readonly UserAccountRepository _userAccountRepository;

        public TasksViewModel(Project currentProject)
        {
            _userAccountRepository = new UserAccountRepository();

            TasksTodoVM = new TasksTodoViewModel();
            CompletedTasksVM = new CompletedTasksViewModel();
            ProjectNotesVM = new ProjectNotesViewModel();
            ProjectDetailsVM = new ProjectDetailsViewModel(currentProject);

            CurrentView = TasksTodoVM;

            //TasksView child Views
            TasksTodoViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = TasksTodoVM; });
            CompletedTasksViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = CompletedTasksVM; });
            ProjectNotesViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = ProjectNotesVM; });
            ProjectDetailsViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = ProjectDetailsVM; });
            ProjectsViewCommand = new DelegateCommand(NavigateToProjectsView);

            CurrentProject = currentProject;
        }

        public Project CurrentProject { get; set; }
        public object CurrentView { get; set; }

        #region ViewModels (that will navigate with their associated Views)
        public ProjectsViewModel ProjectsVM { get; set; }

        //TasksView child Views
        public TasksTodoViewModel TasksTodoVM { get; set; }
        public CompletedTasksViewModel CompletedTasksVM { get; set; }
        public ProjectNotesViewModel ProjectNotesVM { get; set; }
        public ProjectDetailsViewModel ProjectDetailsVM { get; set; }
        #endregion

        #region Commands
        public ICommand ProjectsViewCommand { get; }
        public ICommand TasksTodoViewCommand { get; }
        public ICommand CompletedTasksViewCommand { get; }
        public ICommand ProjectNotesViewCommand { get; }
        public ICommand ProjectDetailsViewCommand { get; }
        #endregion

        #region Navigation Methods
        public void NavigateToProjectsView()
        {
            ProjectsVM = new ProjectsViewModel(_userAccountRepository.GetById(CurrentProject.User_Id_Fk));
            MainWindowViewModel.CurrentView = ProjectsVM;
        }
        #endregion

    }
}