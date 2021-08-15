using Microsoft.Win32;
using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.ViewModels.Windows;
using System;
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
            SelectedProject = currentProject;

            AddOrChangeLogoCommand = new DelegateCommand(LoadProjectLogo);
            UpdateProjectCommand = new DelegateCommand(Update);
            DeleteProjectCommand = new DelegateCommand(Delete);
        }

        #region Properties
        public Project SelectedProject { get; set; }

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
                    SelectedProject.Logo = System.Drawing.Image.FromFile(openFile.FileName);
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Image Format Error", $"Please select a valid logo.\n \n{ex}", Icon.InvalidProject));
                }
            }
        }

        private void Update()
        {
            if (ProjectIsValid())
            {
                UpdateProjectAndShowResult(_projectRepository.Update(SelectedProject));
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
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Invalid Deadline Date", $"The selected date is invalid. Cannot update this Project. \n\nThe deadline date should be on or after { DateTime.Now.Date:MMMM dd, yyyy} (today).", Icon.InvalidProject));
                return false;
            }

            else if (ProjectStatusCanBeCompleted() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", $"Unable to change this Project's Status to \"Completed\" because there are still {SelectedProject.TaskCount} unfinished task/s remaining.", Icon.InvalidProject));
                return false;
            }

            return true;
        }
        private bool ProjectNameIsValid()
        {
            return !string.IsNullOrWhiteSpace(SelectedProject.Name);
        }
        private bool ProjectDeadlineDateIsValid()
        {
            return SelectedProject.Deadline >= DateTime.Now.Date || SelectedProject.Deadline is null;
        }
        private bool ProjectStatusCanBeCompleted()
        {
            //A Project's status can only be changed as "Completed" if they don't have any unfinished Tasks 
            if (SelectedProject.Status == "Completed")
            {
                return SelectedProject.TaskCount == 0;
            }

            return true;
        }
        private void UpdateProjectAndShowResult(bool isValid)
        {
            if (isValid)
            {
                _projectsViewModel.RefreshProjects();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", "Project Updated Successfully!", Icon.ValidProject));
                return;
            }

            _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot update the Project.", Icon.InvalidProject));
        }

        private void Delete()
        {
            DialogResults result = _dialogService.OpenDialog(new YesNoMessageBoxViewModel("Delete Operation",
                                    $"Do you want to DELETE {SelectedProject.Name}? \n\nAll Tasks, Subtasks, and Notes that belongs to this Project will also be deleted.",
                                    Icon.Project));

            if (result == DialogResults.Yes)
            {
                DeleteProject();
            }
        }
        private void DeleteProject()
        {
            bool isDeleted = _projectRepository.Delete(SelectedProject.Id);
            if (isDeleted)
            {
                //redirect to ProjectsView after a successful DELETE operation, and refresh the View with the appropriate records
                _projectsViewModel.RefreshProjects();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Delete Operation", "Project Deleted Successfully!", Icon.ValidProject));
                MainWindowViewModel.CurrentView = _projectsViewModel;
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot delete the Project.", Icon.InvalidProject));
            }
        }
        #endregion
    }
}
