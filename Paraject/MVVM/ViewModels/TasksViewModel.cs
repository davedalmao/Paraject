using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private readonly ProjectsViewModel _projectsViewModel;
        private readonly Action _refreshProjectsCollection;

        public TasksViewModel(ProjectsViewModel projectsViewModel, Action refreshProjectsCollection, Project currentProject)
        {
            _projectsViewModel = projectsViewModel;
            _refreshProjectsCollection = refreshProjectsCollection;
            CurrentProject = currentProject;

            //TasksView child Views (ViewModels)
            TasksTodoVM = new TasksTodoViewModel(this, currentProject.Id, currentProject, "Finish_Line");
            CompletedTasksVM = new CompletedTasksViewModel(this, currentProject.Id, currentProject);
            NotesVM = new NotesViewModel(currentProject.Id);
            ProjectDetailsVM = new ProjectDetailsViewModel(projectsViewModel, currentProject);

            CurrentView = TasksTodoVM;

            //TasksView child Views (Navigation)
            TasksTodoViewCommand = new ParameterizedDelegateCommand(NavigateToTasksTodoView);
            CompletedTasksViewCommand = new DelegateCommand(NavigateToCompletedTasksView);
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
        public TasksTodoViewModel TasksTodoVM { get; set; }
        public CompletedTasksViewModel CompletedTasksVM { get; set; }
        public NotesViewModel NotesVM { get; set; }
        public ProjectDetailsViewModel ProjectDetailsVM { get; set; }

        //Commands
        public ICommand NavigateBackToProjectsViewCommand { get; }
        public ICommand TasksTodoViewCommand { get; }
        public ICommand CompletedTasksViewCommand { get; }
        public ICommand ProjectNotesViewCommand { get; }
        public ICommand ProjectDetailsViewCommand { get; }
        #endregion

        #region Navigation Methods
        public void NavigateBackToProjectsView()
        {
            _refreshProjectsCollection();
            MainWindowViewModel.CurrentView = _projectsViewModel;
        }
        private void NavigateToTasksTodoView(object taskType) //the argument passed to this parameter is in TasksView (a "CommandParameter" from a Tab header)
        {
            TaskHeaderTextIsVisible = true;

            TasksTodoVM = new TasksTodoViewModel(this, CurrentProject.Id, CurrentProject, taskType.ToString());
            CurrentView = TasksTodoVM;
        }
        private void NavigateToCompletedTasksView()
        {
            TaskHeaderTextIsVisible = true;

            CompletedTasksVM = new CompletedTasksViewModel(this, CurrentProject.Id, CurrentProject);
            CurrentView = CompletedTasksVM;
        }
        #endregion
    }
}