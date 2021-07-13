using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class NoteDetailsModalDialogViewModel : BaseViewModel
    {
        private readonly int _noteId;
        private readonly NoteRepository _noteRepository;

        public NoteDetailsModalDialogViewModel(int noteId)
        {
            _noteRepository = new NoteRepository();
            _noteId = noteId;

            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);

            CurrentNote = _noteRepository.Get(noteId);
        }

        #region Properties
        public Note CurrentNote { get; set; }
        public ICommand CloseModalDialogCommand { get; }
        public ICommand UpdateNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        #endregion

        #region Methods
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
