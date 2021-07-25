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
    public class SubtaskDetailsModalDialogViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly SubtaskRepository _subtaskRepository;
        private readonly Action _refreshSubtasksCollection;
        private readonly int _selectedSubtaskId;

        public SubtaskDetailsModalDialogViewModel(Action refreshSubtasksCollection, Task parentTask, int selectedSubtaskId)
        {
            _dialogService = new DialogService();
            _subtaskRepository = new SubtaskRepository();
            _refreshSubtasksCollection = refreshSubtasksCollection;
            _selectedSubtaskId = selectedSubtaskId;
            ParentTask = parentTask;

            UpdateSubtaskCommand = new DelegateCommand(Update);
            DeleteSubtaskCommand = new DelegateCommand(Delete);
            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);

            SelectedSubtask = _subtaskRepository.Get(selectedSubtaskId);
            PreviousSelectedSubtaskStatus = SelectedSubtask.Status;
        }

        #region Properties
        public Task ParentTask { get; set; }
        public Subtask SelectedSubtask { get; set; }

        public string PreviousSelectedSubtaskStatus { get; set; }

        public ICommand UpdateSubtaskCommand { get; }
        public ICommand DeleteSubtaskCommand { get; }
        public ICommand CloseModalDialogCommand { get; }
        #endregion

        #region Methods
        private void Update()
        {
            if (SubtaskIsValid())
            {
                UpdateSubtaskAndShowResult(_subtaskRepository.Update(SelectedSubtask));
            }
        }
        private bool SubtaskIsValid()
        {
            if (SubtaskSubjectIsValid() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Subtask should have a subject.", Icon.InvalidSubtask));
                return false;
            }

            else if (SubtaskStatusCanBeChangedFromCompletedToOtherStatus() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", $"Unable to change this Subtask's status as \"{SelectedSubtask.Status.Replace("_", " ")}\" because the Task's Status that this subtask belongs to is now completed. \n\nChange parent Task's status to \"Open\" or \"In Progress\" to change this subtask's status.", Icon.InvalidSubtask));
                return false;
            }

            UpdateSubtaskCount();
            return true;
        }
        private bool SubtaskSubjectIsValid()
        {
            return !string.IsNullOrWhiteSpace(SelectedSubtask.Subject);
        }
        private bool SubtaskStatusCanBeChangedFromCompletedToOtherStatus()
        {
            if (ParentTask.Status != "Completed")
            {
                return true;
            }

            return SelectedSubtask.Status == "Completed"; //The selected subtask's status should be "Completed" so we can update its other information
        }
        private void UpdateSubtaskCount()
        {
            //if the Previous SelecedSubtask's Status is "Completed" and we change the subtask's status to Open or In Progress, +! to the parent's subtask count (add 1 more "Incomplete" subtask)
            if (PreviousSelectedSubtaskStatus == "Completed")
            {
                IncreaseSubtaskCountIfSubtaskIsUnfinished();
            }

            //if the Previous SelecedSubtask's Status is "Open" or "In Progress", and we change the subtask's status to Completed, -1 to the parent's subtask count (minus 1  "Incomplete" subtask)
            else
            {
                DecreaseSubtaskCountIfSubtaskIsCompleted();
            }
        }
        private void IncreaseSubtaskCountIfSubtaskIsUnfinished()
        {
            if (SelectedSubtask.Status != "Completed")
            {
                ParentTask.SubtaskCount += 1;
            }
        }
        private void DecreaseSubtaskCountIfSubtaskIsCompleted()
        {
            if (SelectedSubtask.Status == "Completed")
            {
                ParentTask.SubtaskCount -= 1;
            }
        }
        private void UpdateSubtaskAndShowResult(bool isUpdated)
        {
            if (isUpdated)
            {
                _refreshSubtasksCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", "Subtask Updated Successfully!", Icon.ValidSubtask));
                CloseModalDialog();
                return;
            }

            _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot update the Subtask.", Icon.InvalidSubtask));
        }

        private void Delete()
        {
            DialogResults result = _dialogService.OpenDialog(new YesNoMessageBoxViewModel("Delete Operation", "Do you want to DELETE this Subtask?", Icon.Subtask));

            if (result == DialogResults.Yes)
            {
                DeleteSubtask();
            }
        }
        private void DeleteSubtask()
        {
            bool isDeleted = _subtaskRepository.Delete(_selectedSubtaskId);
            if (isDeleted)
            {
                _refreshSubtasksCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Delete Operation", "Subtask Deleted Successfully!", Icon.ValidSubtask));
                CloseModalDialog();
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot delete the Subtask.", Icon.InvalidSubtask));
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
