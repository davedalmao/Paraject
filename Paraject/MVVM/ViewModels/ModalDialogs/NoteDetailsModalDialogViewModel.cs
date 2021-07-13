using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class NoteDetailsModalDialogViewModel : BaseViewModel
    {
        private readonly Action _refreshNotesCollection;
        private readonly int _noteId;
        private readonly NoteRepository _noteRepository;

        public NoteDetailsModalDialogViewModel(Action refreshNotesCollection, int noteId)
        {
            _noteRepository = new NoteRepository();
            _refreshNotesCollection = refreshNotesCollection;
            _noteId = noteId;

            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);
            UpdateNoteCommand = new DelegateCommand(Update);
            DeleteNoteCommand = new DelegateCommand(Delete);

            CurrentNote = _noteRepository.Get(noteId);
        }

        #region Properties
        public Note CurrentNote { get; set; }

        public ICommand UpdateNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        public ICommand CloseModalDialogCommand { get; }
        #endregion

        #region Methods
        private void Update()
        {
            if (!string.IsNullOrWhiteSpace(CurrentNote.Subject))
            {
                bool isUpdated = _noteRepository.Update(CurrentNote);
                UpdateOperationResult(isUpdated);
            }
            else
            {
                MessageBox.Show("A Note should have a subject");
            }
        }
        private void UpdateOperationResult(bool isUpdated)
        {
            if (isUpdated)
            {
                _refreshNotesCollection();
                MessageBox.Show("Note updated successfully");
                CloseModalDialog();
            }
            else
            {
                MessageBox.Show("Error occured, cannot update the note");
            }
        }

        private void Delete()
        {
            MessageBoxResult Result = MessageBox.Show("Do you want to DELETE this note?", "Delete Operation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                DeleteNote();
            }
        }
        private void DeleteNote()
        {
            bool isDeleted = _noteRepository.Delete(_noteId);
            if (isDeleted)
            {
                _refreshNotesCollection();
                MessageBox.Show("Note deleted successfully");
                CloseModalDialog();
            }
            else
            {
                MessageBox.Show("An error occurred, cannot delete Note");
            }
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
