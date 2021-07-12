using Paraject.MVVM.Models;

namespace Paraject.MVVM.ViewModels
{
    public class NoteModalDialogViewModel : BaseViewModel
    {
        //Add A Note for this Project
        public NoteModalDialogViewModel(int currentProjectId, string modalFunctionality)
        {
            CurrentNote = new Note();

            ModalDisplay(modalFunctionality);
        }
        public Note CurrentNote { get; set; }
        public string ModalHeaderText { get; set; }

        public bool IsAddNoteActive { get; set; }
        public bool IsModifyNoteActive { get; set; }

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
    }
}
