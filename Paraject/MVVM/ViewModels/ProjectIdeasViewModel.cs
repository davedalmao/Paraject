using Paraject.Core.Commands;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectIdeasViewModel
    {
        private readonly int _currentUserId;

        public ProjectIdeasViewModel(int currentUserId)
        {
            ShowAddProjectIdeaModalDialogCommand = new DelegateCommand(ShowAddProjectIdeaModalDialog);
            _currentUserId = currentUserId;
        }

        public ICommand ShowAddProjectIdeaModalDialogCommand { get; }

        private void ShowAddProjectIdeaModalDialog()
        {
            MainWindowViewModel.Overlay = true;

            AddProjectIdeaModalDialogViewModel addProjectIdeaModalDialogViewModel = new AddProjectIdeaModalDialogViewModel(_currentUserId);

            AddProjectIdeaModalDialog addProjectIdeaModalDialog = new AddProjectIdeaModalDialog();
            addProjectIdeaModalDialog.DataContext = addProjectIdeaModalDialogViewModel;
            addProjectIdeaModalDialog.ShowDialog();
        }
    }
}
