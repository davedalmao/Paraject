using Paraject.Core.Commands;
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
    public class AddProjectIdeaModalDialogViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ProjectIdeaRepository _projectIdeaRepository;
        private readonly Action _refreshProjectIdeasCollection;

        public AddProjectIdeaModalDialogViewModel(Action refreshProjectIdeasCollection, int currentUserId)
        {
            _dialogService = new DialogService();
            _projectIdeaRepository = new ProjectIdeaRepository();
            CurrentProjectIdea = new ProjectIdea()
            {
                User_Id_Fk = currentUserId
            };

            _refreshProjectIdeasCollection = refreshProjectIdeasCollection;

            AddProjectIdeaCommand = new DelegateCommand(Add);
            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);
        }

        #region Properties
        public ProjectIdea CurrentProjectIdea { get; set; }
        public ICommand AddProjectIdeaCommand { get; }
        public ICommand CloseModalDialogCommand { get; }
        #endregion

        #region Methods
        private void Add()
        {
            if (!string.IsNullOrWhiteSpace(CurrentProjectIdea.Name))
            {
                bool isAdded = _projectIdeaRepository.Add(CurrentProjectIdea);
                AddOperationResult(isAdded);
            }

            else
            {
                string iconSource = "/UiDesign/Images/Logo/defaultProjectLogo.png";

                OkayMessageBoxViewModel okayMessageBox = new("Incorrect Data Entry", "A Subtask should have a subject.", iconSource);
                _dialogService.OpenDialog(okayMessageBox);
            }
        }
        private void AddOperationResult(bool isAdded)
        {
            string iconSource = "/UiDesign/Images/Logo/defaultProjectLogo.png";

            if (isAdded)
            {
                _refreshProjectIdeasCollection();

                OkayMessageBoxViewModel okayMessageBox = new("Add Operation", "Project Idea Created Successfully!", iconSource);
                _dialogService.OpenDialog(okayMessageBox);

                CloseModalDialog();
            }

            else
            {
                OkayMessageBoxViewModel okayMessageBox = new("Error", "An error occured, cannot create the Project Idea.", iconSource);
                _dialogService.OpenDialog(okayMessageBox);
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
