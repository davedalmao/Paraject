using Microsoft.Win32;
using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectDetailsViewModel : BaseViewModel
    {
        private readonly ProjectRepository _projectRepository;
        private readonly ProjectsViewModel _projectsViewModel;

        public ProjectDetailsViewModel(ProjectsViewModel projectsViewModel, Project currentProject)
        {
            _projectsViewModel = projectsViewModel;
            _projectRepository = new ProjectRepository();
            CurrentProject = currentProject;

            //Commands
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
                CurrentProject.Logo = Image.FromFile(openFile.FileName);
            }
        }
        private void Update()
        {
            if (!string.IsNullOrWhiteSpace(CurrentProject.Name))
            {
                bool isUpdated = _projectRepository.Update(CurrentProject);
                if (isUpdated)
                {
                    MessageBox.Show("Project updated successfully");
                }
                else
                {
                    MessageBox.Show("Error occured, cannot update the project");
                }
            }
            else
            {
                MessageBox.Show("A Project should have a name");
            }
        }
        private void Delete()
        {
            MessageBoxResult Result = MessageBox.Show($"Do you want to DELETE {CurrentProject.Name}? \n\nAll Tasks, Subtasks, and Notes that belongs to this Project will also be deleted.", "Delete Operation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (Result == MessageBoxResult.Yes)
            {
                DeleteProject();
            }
        }
        private void DeleteProject()
        {
            bool isDeleted = _projectRepository.Delete(CurrentProject.Id);
            if (isDeleted)
            {
                MessageBox.Show("Project deleted successfully");

                //redirect to ProjectsView after a successful DELETE operation, and refresh the View with the appropriate records
                _projectsViewModel.RefreshProjects();
                MainWindowViewModel.CurrentView = _projectsViewModel;
            }
            else
            {
                MessageBox.Show("An error occurred, cannot delete project");
            }
        }
        #endregion
    }
}
