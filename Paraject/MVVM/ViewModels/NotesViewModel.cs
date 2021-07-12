using Paraject.Core.Commands;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        public NotesViewModel()
        {
            ShowNoteModalDialogCommand = new DelegateCommand(ShowNoteModalDialog);
        }

        public ICommand ShowNoteModalDialogCommand { get; }

        public void ShowNoteModalDialog()
        {
            MainWindowViewModel.Overlay = true;

            NoteModalDialog noteModalDialog = new NoteModalDialog();
            noteModalDialog.ShowDialog();
        }
    }
}
