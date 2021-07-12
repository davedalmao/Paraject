using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class NoteModalDialogViewModel : BaseViewModel
    {
        private readonly NoteRepository _noteRepository;
        private readonly Action _refreshNotesCollection;
        private readonly int _currentProjectId;

        public NoteModalDialogViewModel(Action refreshNotesCollection, int currentProjectId, ModalFunctionality modalFunctionality)
        {
            _noteRepository = new NoteRepository();
            _refreshNotesCollection = refreshNotesCollection;
            _currentProjectId = currentProjectId;
            CurrentNote = new Note();

            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);
            AddNoteCommand = new DelegateCommand(Add);

            ModalDisplay(modalFunctionality);
        }

        #region Properties
        public Note CurrentNote { get; set; }
        public string ModalHeaderText { get; set; }

        public bool IsAddNoteActive { get; set; }
        public bool IsModifyNoteActive { get; set; }

        public ICommand AddNoteCommand { get; }
        public ICommand CloseModalDialogCommand { get; }
        #endregion

        #region Methods
        public void Add()
        {
            if (!string.IsNullOrWhiteSpace(CurrentNote.Subject))
            {
                bool isAdded = _noteRepository.Add(CurrentNote, _currentProjectId);
                AddOperationResult(isAdded);
            }

            else
            {
                MessageBox.Show("A note should have a subject");
            }
        }
        private void AddOperationResult(bool isAdded)
        {
            if (isAdded)
            {
                //refresh Notes collection in NotesView//
                _refreshNotesCollection();
                MessageBox.Show("Note Created");
                CloseModalDialog();
            }

            else
            {
                MessageBox.Show("Error occured, cannot create note");
            }
        }

        public void ModalDisplay(ModalFunctionality modalFunctionality)
        {
            if (modalFunctionality == ModalFunctionality.Add)
            {
                ModalHeaderText = "Add a Note for this Project";
                IsAddNoteActive = true;
                IsModifyNoteActive = false;
            }

            else if (modalFunctionality == ModalFunctionality.Modify)
            {
                ModalHeaderText = "Note’s Details";
                IsModifyNoteActive = true;
                IsAddNoteActive = false;
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
