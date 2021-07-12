using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        private readonly int _currentProjectId;

        public NotesViewModel(int currentProjectId)
        {
            ShowNoteModalDialogCommand = new DelegateCommand(ShowNoteModalDialog);
            _currentProjectId = currentProjectId;
        }

        public ICommand ShowNoteModalDialogCommand { get; }

        public void ShowNoteModalDialog()
        {
            MainWindowViewModel.Overlay = true;

            NoteModalDialogViewModel noteModalDialogViewModel = new NoteModalDialogViewModel(_currentProjectId, ModalFunctionality.Add);

            NoteModalDialog noteModalDialog = new()
            {
                DataContext = noteModalDialogViewModel
            };
            noteModalDialog.ShowDialog();
        }
    }
}
