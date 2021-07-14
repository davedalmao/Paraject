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
            UpdateProjectIdeaCommand = new DelegateCommand(Update);
            DeleteProjectIdeaCommand = new DelegateCommand(Delete);

            CurrentProjectIdea = _projectIdeaRepository.Get(projectIdeaId);
        }

        #region Properties
        public ProjectIdea CurrentProjectIdea { get; set; }

        public ICommand UpdateProjectIdeaCommand { get; }
        public ICommand DeleteProjectIdeaCommand { get; }
        public ICommand CloseModalDialogCommand { get; }

        #endregion

        #region Methods
        private void Update()
        {
            if (!string.IsNullOrWhiteSpace(CurrentProjectIdea.Name))
            {
                bool isUpdated = _projectIdeaRepository.Update(CurrentProjectIdea);
                UpdateOperationResult(isUpdated);
            }
            else
            {
                MessageBox.Show("A Project Idea should have a name");
            }
        }
        private void UpdateOperationResult(bool isUpdated)
        {
            if (isUpdated)
            {
                // _refreshNotesCollection();
                MessageBox.Show("Project Idea updated successfully");
                CloseModalDialog();
            }
            else
            {
                MessageBox.Show("Error occured, cannot update the Project Idea");
            }
        }

        private void Delete()
        {
            MessageBoxResult Result = MessageBox.Show("Do you want to DELETE this note?", "Delete Operation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                DeleteNote();
            }
        }
        private void DeleteNote()
        {
            bool isDeleted = _projectIdeaRepository.Delete(_projectIdeaId);
            if (isDeleted)
            {
                //_refreshNotesCollection();
                MessageBox.Show("Note deleted successfully");
                CloseModalDialog();
            }
            else
            {
                MessageBox.Show("An error occurred, cannot delete Note");
            }
        }

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
