using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class AddSubtaskModalDialogViewModel : BaseViewModel
    {
        private readonly SubtaskRepository _subtaskRepository;
        private readonly Action _refreshSubtasksCollection;
        private readonly int _currentTaskId;

        public AddSubtaskModalDialogViewModel(int currentTaskId, Action refreshSubtasksCollection)
        {
            _subtaskRepository = new SubtaskRepository();
            _refreshSubtasksCollection = refreshSubtasksCollection;
            _currentTaskId = currentTaskId;

            CurrentSubtask = new Subtask();
            AddSubtaskCommand = new DelegateCommand(Add);
            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);
        }

        #region Properties
        public Subtask CurrentSubtask { get; set; }
        public ICommand CloseModalDialogCommand { get; }
        public ICommand AddSubtaskCommand { get; }
        #endregion

        #region Methods
        public void Add()
        {
            if (!string.IsNullOrWhiteSpace(CurrentSubtask.Subject))
            {
                bool isAdded = _subtaskRepository.Add(CurrentSubtask, _currentTaskId);
                AddOperationResult(isAdded);
            }

            else
            {
                MessageBox.Show("A subtask should have a subject");
            }
        }
        private void AddOperationResult(bool isAdded)
        {
            if (isAdded)
            {
                //messagebox issue (this is just temporary, we're going to use a custom MessageBox anyway)
                _refreshSubtasksCollection();
                MessageBox.Show("Subtask Created");
                CloseModalDialog();
            }

            else
            {
                MessageBox.Show("Error occured, cannot create subtask");
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
