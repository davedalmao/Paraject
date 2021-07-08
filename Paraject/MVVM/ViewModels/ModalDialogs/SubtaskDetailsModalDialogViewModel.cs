using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class SubtaskDetailsModalDialogViewModel : BaseViewModel
    {
        private readonly SubtaskRepository _subtaskRepository;
        private readonly Action _refreshSubtasksCollection;
        private readonly int _subtaskId;

        public SubtaskDetailsModalDialogViewModel(Action refreshSubtasksCollection, int subtaskId)
        {
            _subtaskRepository = new SubtaskRepository();
            _refreshSubtasksCollection = refreshSubtasksCollection;
            _subtaskId = subtaskId;

            CurrentSubtask = new Subtask();

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
                MessageBox.Show("A subtask should have a subject");
            }
        }
        private void UpdateOperationResult(bool isUpdated)
        {
            if (isUpdated)
            {
                _refreshSubtasksCollection();
                MessageBox.Show("Subtask updated successfully");
                CloseModalDialog();
            }
            else
            {
                MessageBox.Show("Error occured, cannot update the subtask");
            }
        }
        private void Delete()
        {
            MessageBox.Show(_subtaskId.ToString());
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
