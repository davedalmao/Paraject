using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectContentViewModel : BaseViewModel
    {
        private readonly ProjectsViewModel _projectsViewModel;
        private readonly Action _refreshProjectsCollection;

        public ProjectContentViewModel(ProjectsViewModel projectsViewModel, Action refreshProjectsCollection, Project currentProject)
        {
            _projectsViewModel = projectsViewModel;
            _refreshProjectsCollection = refreshProjectsCollection;
            CurrentProject = currentProject;

            //TasksView child Views (ViewModels)
            TasksVM = new TasksViewModel(this, currentProject, "Finish_Line");
            NotesVM = new NotesViewModel(currentProject.Id);
            ProjectDetailsVM = new ProjectDetailsViewModel(projectsViewModel, currentProject);

            CurrentView = TasksVM;

            //TasksView child Views (Navigation)
            TasksViewCommand = new ParameterizedDelegateCommand(NavigateToTasksView);
            ProjectNotesViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = NotesVM; TaskHeaderTextIsVisible = false; });
            ProjectDetailsViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = ProjectDetailsVM; TaskHeaderTextIsVisible = false; });
            NavigateBackToProjectsViewCommand = new DelegateCommand(NavigateBackToProjectsView);
        }

        #region Properties
        public Project CurrentProject { get; set; }
        public object CurrentView { get; set; }

        //RadioButtons in TasksView
        public bool FinishLineButtonIsChecked { get; set; } = true; //default selected RadioButton
        public bool ExtraFeaturesButtonIsChecked { get; set; }
        public bool CompletedButtonIsChecked { get; set; }

        public bool TaskHeaderTextIsVisible { get; set; } = true;

        //TasksView child Views
        public TasksViewModel TasksVM { get; set; }
        public NotesViewModel NotesVM { get; set; }
        public ProjectDetailsViewModel ProjectDetailsVM { get; set; }

        //Commands
        public ICommand TasksViewCommand { get; }
        public ICommand ProjectNotesViewCommand { get; }
        public ICommand ProjectDetailsViewCommand { get; }
        public ICommand NavigateBackToProjectsViewCommand { get; }
        #endregion

        #region Methods
        public void NavigateBackToProjectsView()
        {
            _refreshProjectsCollection();
            MainWindowViewModel.CurrentView = _projectsViewModel;
        }
        private void NavigateToTasksView(object taskType) //the argument passed to this parameter is in ProjectContentView (a "CommandParameter" from a RadioButton)
        {
            TaskHeaderTextIsVisible = true;

            TasksVM = new TasksViewModel(this, CurrentProject, taskType?.ToString());
            CurrentView = TasksVM;
        }
        #endregion
    }
}