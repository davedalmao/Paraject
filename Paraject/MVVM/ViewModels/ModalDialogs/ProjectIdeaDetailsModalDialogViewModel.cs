using Paraject.MVVM.Models;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class ProjectIdeaDetailsModalDialogViewModel : BaseViewModel
    {
        private readonly int _projectIdeaId;

        public ProjectIdeaDetailsModalDialogViewModel(int projectIdeaId)
        {
            _projectIdeaId = projectIdeaId;
        }

        #region Properties
        public ProjectIdea CurrentProjectIdea { get; set; }

        public ICommand UpdateProjectIdeaCommand { get; }
        public ICommand DeleteProjectIdeaCommand { get; }
        #endregion
    }
}
