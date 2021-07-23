using Microsoft.Win32;
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
    public class AddProjectModalDialogViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ProjectRepository _projectRepository;
        private readonly Action _refreshProjectsCollection;

        public AddProjectModalDialogViewModel(Action refreshProjectsCollection, int currentUserId)
        {
            _dialogService = new DialogService();
            _projectRepository = new ProjectRepository();
            _refreshProjectsCollection = refreshProjectsCollection;

            CurrentProject = new Project()
            {
                User_Id_Fk = currentUserId
            };

            AddProjectCommand = new DelegateCommand(Add);
            AddProjectLogoCommand = new DelegateCommand(LoadProjectLogo);
            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);
        }

        #region Properties
        public Project CurrentProject { get; set; }

        public ICommand AddProjectCommand { get; }
        public ICommand AddProjectLogoCommand { get; }
        public ICommand CloseModalDialogCommand { get; }
        #endregion

        #region Methods
        public void Add()
        {
            if (ProjectIsValid())
            {
                AddProjectToDatabaseAndShowResult(_projectRepository.Add(CurrentProject));
            }
        }
        private bool ProjectIsValid()
        {
            if (ProjectNameIsValid() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Project should have a name.", Icon.InvalidProject));
                return false;
            }

            else if (ProjectDeadlineDateIsValid() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Invalid Deadline Date", "The selected date is invalid. Cannot create a new Project.", Icon.InvalidProject));
                return false;
            }

            return true;
        }
        private bool ProjectNameIsValid()
        {
            return !string.IsNullOrWhiteSpace(CurrentProject.Name);
        }
        private bool ProjectDeadlineDateIsValid()
        {
            return CurrentProject.Deadline >= DateTime.Now.Date || CurrentProject.Deadline is null;
        }
        private void AddProjectToDatabaseAndShowResult(bool isValid)
        {
            if (isValid)
            {
                _refreshProjectsCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Add Operation", "Project Created Successfully!", Icon.ValidProject));
                CloseModalDialog();
                return;
            }

            _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot create the Project.", Icon.InvalidProject));
        }

        private void LoadProjectLogo()
        {
            OpenFileDialog openFile = new()
            {
                Title = "Select The Project's Logo",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    CurrentProject.Logo = System.Drawing.Image.FromFile(openFile.FileName);
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Image Format Error", $"Please select a valid image.\n \n{ex}", Icon.InvalidProject));
                }
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
