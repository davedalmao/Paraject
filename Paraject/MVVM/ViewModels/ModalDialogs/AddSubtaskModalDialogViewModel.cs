using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class AddSubtaskModalDialogViewModel : BaseViewModel, ICloseWindows
    {
        private readonly IDialogService _dialogService;
        private readonly SubtaskRepository _subtaskRepository;
        private readonly Action _refreshSubtasksCollection;
        private DelegateCommand _closeCommand;

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
        public Action Close { get; set; }

        public ICommand AddSubtaskCommand { get; }
        public ICommand CloseCommand => _closeCommand ??= new DelegateCommand(CloseWindow);
        #endregion

        #region Methods
        public void Add()
        {
            if (!string.IsNullOrWhiteSpace(CurrentSubtask.Subject))
            {
                bool isAdded = _subtaskRepository.Add(CurrentSubtask);
                AddOperationResult(isAdded);
            }

            else
            {
                string iconSource = "/UiDesign/Images/Logo/defaultProjectLogo.png";
                OkayMessageBoxViewModel okayMessageBox = new("Data Entry", "A subtask should have a subject", iconSource);
                _dialogService.OpenDialog(okayMessageBox);
            }
        }
        private void AddOperationResult(bool isAdded)
        {
            string iconSource = "/UiDesign/Images/Logo/defaultProjectLogo.png";
            if (isAdded)
            {
                OkayMessageBoxViewModel okayMessageBox = new("Add Operation", "Subtask Created Successfully!", iconSource);

                _refreshSubtasksCollection();
                _dialogService.OpenDialog(okayMessageBox);
                CloseWindow();
            }

            else
            {
                OkayMessageBoxViewModel okayMessageBox = new("Add Operation", "An Error occured, cannot create the Subtask ;(", iconSource);
                _dialogService.OpenDialog(okayMessageBox);
            }
        }
        public void CloseWindow()
        {
            Close?.Invoke();
            MainWindowViewModel.Overlay = false;
        }
        #endregion
    }
}
