using Microsoft.Win32;
using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectDetailsViewModel : BaseViewModel
    {
        private readonly ProjectRepository _projectRepository;
        public ProjectDetailsViewModel(Project currentProject)
        {
            //Repository
            _projectRepository = new ProjectRepository();

            //Project Model
            CurrentProject = currentProject;

            //Commands
            AddOrChangeLogoCommand = new DelegateCommand(LoadProjectLogo);
            UpdateProjectCommand = new DelegateCommand(Update);
            DeleteProjectCommand = new DelegateCommand(Delete);
        }

        #region Properties
        //Model
        public Project CurrentProject { get; set; }

        //Commands
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
            MessageBox.Show("Delete");
        }
        #endregion
    }
}
