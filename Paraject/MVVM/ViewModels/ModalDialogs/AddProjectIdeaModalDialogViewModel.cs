using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class AddProjectIdeaModalDialogViewModel : BaseViewModel
    {
        public AddProjectIdeaModalDialogViewModel(int currentUserId)
        {
            CurrentProjectIdea = new ProjectIdea();
            AddProjectIdeaCommand = new DelegateCommand(Add);
        }

        public ProjectIdea CurrentProjectIdea { get; set; }
        public ICommand AddProjectIdeaCommand { get; }

        private void Add()
        {
            MessageBox.Show($"Project Name: {CurrentProjectIdea.Name} \nDescription: {CurrentProjectIdea.Description} \nFeatures: {CurrentProjectIdea.Features}");
        }
    }
}
