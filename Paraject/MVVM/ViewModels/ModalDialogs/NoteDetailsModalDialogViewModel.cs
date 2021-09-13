using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class NoteDetailsModalDialogViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly Action _refreshNotesCollection;
        private readonly int _noteId;
        private readonly NoteRepository _noteRepository;

        public NoteDetailsModalDialogViewModel(Action refreshNotesCollection, int noteId)
        {
            _dialogService = new DialogService();
            _noteRepository = new NoteRepository();
            _refreshNotesCollection = refreshNotesCollection;
            _noteId = noteId;

            CloseModalDialogCommand = new RelayCommand(CloseModalDialog);
            UpdateNoteCommand = new RelayCommand(Update);
            DeleteNoteCommand = new RelayCommand(Delete);

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
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Note should have a subject.", Icon.InvalidNote));
            }
        }
        private void UpdateOperationResult(bool isUpdated)
        {
            if (isUpdated)
            {
                _refreshNotesCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", "Note Updated Successfully!", Icon.ValidNote));
                CloseModalDialog();
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot update the Note.", Icon.InvalidNote));
            }
        }

        private void Delete()
        {
            DialogResults result = _dialogService.OpenDialog(new YesNoMessageBoxViewModel("Delete Operation", "Do you want to DELETE this Note?", Icon.Note));

            if (result == DialogResults.Yes)
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
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Delete Operation", "Note Deleted Successfully!", Icon.ValidNote));
                CloseModalDialog();
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Delete Operation", "An error occurred, cannot delete the Note.", Icon.InvalidNote));
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
