using Microsoft.Win32;
using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using System.Drawing;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectDetailsViewModel : BaseViewModel
    {
        public ProjectDetailsViewModel(Project currentProject)
        {
            CurrentProject = currentProject;
            AddOrChangeLogoCommand = new DelegateCommand(LoadProjectLogo);
        }

        public Project CurrentProject { get; set; }
        public ICommand AddOrChangeLogoCommand { get; }
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
    }
}
