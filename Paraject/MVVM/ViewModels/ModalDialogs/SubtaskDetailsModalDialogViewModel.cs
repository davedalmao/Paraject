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
        private readonly int _subtaskId;

        public SubtaskDetailsModalDialogViewModel(Action refreshSubtasksCollection, int subtaskId)
        {
            _dialogService = new DialogService();
            _subtaskRepository = new SubtaskRepository();
            _refreshSubtasksCollection = refreshSubtasksCollection;
            _subtaskId = subtaskId;

            UpdateSubtaskCommand = new DelegateCommand(Update);
            DeleteSubtaskCommand = new DelegateCommand(Delete);
            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);

            CurrentSubtask = _subtaskRepository.Get(subtaskId);
        }

        #region Properties
        public Subtask CurrentSubtask { get; set; }

        public ICommand UpdateSubtaskCommand { get; }
        public ICommand DeleteSubtaskCommand { get; }
        public ICommand CloseModalDialogCommand { get; }
        #endregion

        #region Methods
        private void Update()
        {
            if (!string.IsNullOrWhiteSpace(CurrentSubtask.Subject))
            {
                bool isUpdated = _subtaskRepository.Update(CurrentSubtask);
                UpdateOperationResult(isUpdated);
            }

            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Subtask should have a subject.", Icon.InvalidSubtask));
            }
        }
        private void UpdateOperationResult(bool isUpdated)
        {
            if (isUpdated)
            {
                _refreshSubtasksCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", "Subtask Updated Successfully!", Icon.ValidSubtask));
                CloseModalDialog();
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot update the Subtask.", Icon.InvalidSubtask));
            }
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
            bool isDeleted = _subtaskRepository.Delete(_subtaskId);
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
