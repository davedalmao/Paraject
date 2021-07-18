using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class AddSubtaskModalDialogViewModel : BaseViewModel, ICloseWindows
    {
        private readonly IDialogService _dialogService;
        private readonly SubtaskRepository _subtaskRepository;
        private readonly Action _refreshSubtasksCollection;

        public AddSubtaskModalDialogViewModel(int currentTaskId, Action refreshSubtasksCollection)
        {
            _subtaskRepository = new SubtaskRepository();
            _dialogService = new DialogService();
            _refreshSubtasksCollection = refreshSubtasksCollection;

            CurrentSubtask = new Subtask()
            {
                Task_Id_Fk = currentTaskId
            };

            AddSubtaskCommand = new DelegateCommand(Add);
        }

        #region Properties
        public Subtask CurrentSubtask { get; set; }
        public ICommand AddSubtaskCommand { get; }
        #endregion

        #region Methods
        public void Add()
        {
            if (!string.IsNullOrWhiteSpace(CurrentSubtask.Subject))
            {
                bool isAdded = _subtaskRepository.Add(CurrentSubtask);
                AddOperationResult(isAdded);
            }

            //problem here 
            else
            {
                OkayMessageBoxViewModel okayMessageBox = new("Data Entry", "A subtask should have a subject");
                _dialogService.OpenDialog(okayMessageBox);
            }
        }
        private void AddOperationResult(bool isAdded)
        {
            if (isAdded)
            {
                _refreshSubtasksCollection();
                OkayMessageBoxViewModel okayMessageBox = new("Add Operation", "Subtask Created Successfully!");
                _dialogService.OpenDialog(okayMessageBox);
                CloseWindow();
            }

            else
            {
                //problem here 
                MessageBox.Show("Error occured, cannot create subtask");
            }
        }
        #endregion

        //new
        private DelegateCommand _closeCommand;

        public DelegateCommand CloseCommand => _closeCommand ??= new DelegateCommand(CloseWindow);

        public Action Close { get; set; }

        public void CloseWindow()
        {
            Close?.Invoke();
            MainWindowViewModel.Overlay = false;
        }
    }
}
