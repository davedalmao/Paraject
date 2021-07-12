using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class NoteModalDialogViewModel : BaseViewModel
    {
        //Add A Note for this Project
        public NoteModalDialogViewModel(int currentProjectId, string modalFunctionality)
        {
            CurrentNote = new Note();

            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);

            ModalDisplay(modalFunctionality);
        }
        #region Properties
        public Note CurrentNote { get; set; }
        public string ModalHeaderText { get; set; }

        public bool IsAddNoteActive { get; set; }
        public bool IsModifyNoteActive { get; set; }

        public ICommand CloseModalDialogCommand { get; }
        #endregion

        #region Methods
        public void ModalDisplay(string modalFunctionality)
        {
            if (modalFunctionality == "Add")
            {
                ModalHeaderText = "Add a Note for this Project";
                IsAddNoteActive = true;
                IsModifyNoteActive = false;
            }

            else if (modalFunctionality == "Modify")
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
