using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
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
            MessageBox.Show($"Project Name: {CurrentProjectIdea.Name} \nDescription: {CurrentProjectIdea.Description} \nFeatures: {CurrentProjectIdea.Features}");
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
