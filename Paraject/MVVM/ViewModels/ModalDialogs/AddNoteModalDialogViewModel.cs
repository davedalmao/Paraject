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
    public class AddNoteModalDialogViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly NoteRepository _noteRepository;
        private readonly Action _refreshNotesCollection;

        public AddNoteModalDialogViewModel(Action refreshNotesCollection, int currentProjectId)
        {
            _dialogService = new DialogService();
            _noteRepository = new NoteRepository();
            _refreshNotesCollection = refreshNotesCollection;

            CurrentNote = new Note()
            {
                Project_Id_Fk = currentProjectId
            };

            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);
            AddNoteCommand = new DelegateCommand(Add);
        }

        #region Properties
        public Note CurrentNote { get; set; }

        public ICommand AddNoteCommand { get; }
        public ICommand CloseModalDialogCommand { get; }
        #endregion

        #region Methods
        public void Add()
        {
            if (!string.IsNullOrWhiteSpace(CurrentNote.Subject))
            {
                bool isAdded = _noteRepository.Add(CurrentNote);
                AddOperationResult(isAdded);
            }

            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Note should have a subject.", Icon.InvalidNote));
            }
        }
        private void AddOperationResult(bool isAdded)
        {
            if (isAdded)
            {
                _refreshNotesCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Add Operation", "Note Created Successfully!", Icon.ValidNote));
                CloseModalDialog();
            }

            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot create the Note.", Icon.InvalidNote));
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
