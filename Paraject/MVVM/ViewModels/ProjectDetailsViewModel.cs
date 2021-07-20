using Microsoft.Win32;
using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Drawing;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectDetailsViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ProjectRepository _projectRepository;
        private readonly ProjectsViewModel _projectsViewModel;

        public ProjectDetailsViewModel(ProjectsViewModel projectsViewModel, Project currentProject)
        {
            _dialogService = new DialogService();
            _projectRepository = new ProjectRepository();
            _projectsViewModel = projectsViewModel;
            CurrentProject = currentProject;

            AddOrChangeLogoCommand = new DelegateCommand(LoadProjectLogo);
            UpdateProjectCommand = new DelegateCommand(Update);
            DeleteProjectCommand = new DelegateCommand(Delete);
        }

        #region Properties
        public Project CurrentProject { get; set; }

        public ICommand AddOrChangeLogoCommand { get; }
        public ICommand UpdateProjectCommand { get; }
        public ICommand DeleteProjectCommand { get; }
        #endregion

        #region Methods
        private void LoadProjectLogo()
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Title = "Select the project's logo",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    CurrentProject.Logo = Image.FromFile(openFile.FileName);
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Image Format Error", $"Please select a valid image.\n \n{ex}", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
                }
            }
        }

        private void Update()
        {
            if (!string.IsNullOrWhiteSpace(CurrentProject.Name))
            {
                bool isUpdated = _projectRepository.Update(CurrentProject);
                UpdateOperationResult(isUpdated);
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Project should have a name.", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            }
        }
        private void UpdateOperationResult(bool isUpdated)
        {
            if (isUpdated)
            {
                _projectsViewModel.RefreshProjects();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", "Project Updated Successfully!", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot update the Project.", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            }
        }

        private void Delete()
        {
            DialogResults result = _dialogService.OpenDialog(new YesNoMessageBoxViewModel("Delete Operation",
                                    $"Do you want to DELETE {CurrentProject.Name}? \n\nAll Tasks, Subtasks, and Notes that belongs to this Project will also be deleted.",
                                    "/UiDesign/Images/Logo/defaultProjectLogo.png"));

            if (result == DialogResults.Yes)
            {
                DeleteProject();
            }
        }
        private void DeleteProject()
        {
            bool isDeleted = _projectRepository.Delete(CurrentProject.Id);
            if (isDeleted)
            {
                //redirect to ProjectsView after a successful DELETE operation, and refresh the View with the appropriate records
                _projectsViewModel.RefreshProjects();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Delete Operation", "Project Deleted Successfully!", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
                MainWindowViewModel.CurrentView = _projectsViewModel;
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot delete the Project.", "/UiDesign/Images/Logo/defaultProjectLogo.png"));
            }
        }
        #endregion
    }
}
