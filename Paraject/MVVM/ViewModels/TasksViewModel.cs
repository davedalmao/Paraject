using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        public TasksViewModel(Project currentProject)
        {
            TasksTodoVM = new TasksTodoViewModel();
            CompletedTasksVM = new CompletedTasksViewModel();
            ProjectNotesVM = new ProjectNotesViewModel();
            ProjectDetailsVM = new ProjectDetailsViewModel(currentProject);

            CurrentView = TasksTodoVM;

            TasksTodoViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = TasksTodoVM; });
            CompletedTasksViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = CompletedTasksVM; });
            ProjectNotesViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = ProjectNotesVM; });
            ProjectDetailsViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = ProjectDetailsVM; });

            CurrentProject = currentProject;
        }

        public Project CurrentProject { get; set; }
        public object CurrentView { get; set; }

        #region ViewModels (that will navigate with their associated Views)
        public TasksTodoViewModel TasksTodoVM { get; set; }
        public CompletedTasksViewModel CompletedTasksVM { get; set; }
        public ProjectNotesViewModel ProjectNotesVM { get; set; }
        public ProjectDetailsViewModel ProjectDetailsVM { get; set; }
        #endregion

        #region Commands
        public ICommand TasksTodoViewCommand { get; }
        public ICommand CompletedTasksViewCommand { get; }
        public ICommand ProjectNotesViewCommand { get; }
        public ICommand ProjectDetailsViewCommand { get; }
        #endregion
    }
}