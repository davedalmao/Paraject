using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class ProjectIdeaDetailsModalDialogViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ProjectIdeaRepository _projectIdeaRepository;
        private readonly Action _refreshProjectIdeasCollection;
        private readonly int _projectIdeaId;

        public ProjectIdeaDetailsModalDialogViewModel(Action refreshProjectIdeasCollection, int projectIdeaId)
        {
            _dialogService = new DialogService();
            _projectIdeaRepository = new ProjectIdeaRepository();
            _refreshProjectIdeasCollection = refreshProjectIdeasCollection;
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
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Project Idea should have a name.", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            }
        }
        private void UpdateOperationResult(bool isUpdated)
        {
            if (isUpdated)
            {
                _refreshProjectIdeasCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "Project Idea Updated Successfully!", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
                CloseModalDialog();
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot update the Project Idea.", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            }
        }

        private void Delete()
        {
            DialogResults result = _dialogService.OpenDialog(new YesNoMessageBoxViewModel("Delete Operation", "Do you want to DELETE this Project Idea?", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            if (result == DialogResults.Yes)
            {
                DeleteProjectIdea();
            }
        }
        private void DeleteProjectIdea()
        {
            bool isDeleted = _projectIdeaRepository.Delete(_projectIdeaId);
            if (isDeleted)
            {
                _refreshProjectIdeasCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Delete Operation", "Project Idea Deleted Successfully!", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
                CloseModalDialog();
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot delete the Project Idea.", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
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
