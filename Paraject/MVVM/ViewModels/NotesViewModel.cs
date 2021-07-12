using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        private readonly int _currentProjectId;
        private readonly NoteRepository _noteRepository;

        public NotesViewModel(int currentProjectId)
        {
            _noteRepository = new NoteRepository();
            _currentProjectId = currentProjectId;

            ShowNoteModalDialogCommand = new DelegateCommand(ShowNoteModalDialog);
            DisplayAllNotes();
        }

        #region Properties
        public ObservableCollection<Note> Notes { get; set; }
        public ObservableCollection<GridTileData> NoteCardsGrid { get; set; }

        public ICommand ShowNoteModalDialogCommand { get; }
        #endregion

        #region Methods
        private void DisplayAllNotes()
        {
            GetValuesForNotesCollection();
            SetNewGridDisplay();
            NoteCardsGridLocation();
        }
        private void GetValuesForNotesCollection()
        {
            Notes = new ObservableCollection<Note>(_noteRepository.GetAll(_currentProjectId));
        }
        private void SetNewGridDisplay()
        {
            NoteCardsGrid = null;
            NoteCardsGrid = new ObservableCollection<GridTileData>();
        }
        private void NoteCardsGridLocation()
        {
            int row = -1;
            int column = -1;

            //This is for a 3 column grid, with n number of rows
            for (int i = 0; i < Notes.Count; i++)
            {
                if (column == 2)
                {
                    column = 0;
                }

                else
                {
                    column++;
                }

                if (i % 3 == 0)
                {
                    row++;
                }

                GridTileData td = new(Notes[i], row, column);
                NoteCardsGrid.Add(td);
            }
        }

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
        #endregion
    }
}
