using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class ProjectIdeaDetailsModalDialogViewModel : BaseViewModel
    {
        private readonly int _projectIdeaId;
        private readonly ProjectIdeaRepository _projectIdeaRepository;

        public ProjectIdeaDetailsModalDialogViewModel(int projectIdeaId)
        {
            _projectIdeaRepository = new ProjectIdeaRepository();
            _projectIdeaId = projectIdeaId;

            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);

            CurrentProjectIdea = _projectIdeaRepository.Get(projectIdeaId);
        }

        #region Properties
        public ProjectIdea CurrentProjectIdea { get; set; }

        public ICommand UpdateProjectIdeaCommand { get; }
        public ICommand DeleteProjectIdeaCommand { get; }
        public ICommand CloseModalDialogCommand { get; }

        #endregion

        #region Methods
        private void CloseModalDialog()
        {
            MainWindowViewModel.Overlay = false;

            foreach (Window currentModal in Application.Current.Windows)
            {
                if (currentModal.DataContext == this)
                {
                    currentModal.Close();
                }
            }
        }
        #endregion
    }
}
