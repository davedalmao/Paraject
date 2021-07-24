using Paraject.Core.Commands;
using Paraject.Core.Enums;
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
        private readonly Task _parentTask;

        public AddSubtaskModalDialogViewModel(Task parentTask, Action refreshSubtasksCollection)
        {
            _dialogService = new DialogService();
            _subtaskRepository = new SubtaskRepository();
            _refreshSubtasksCollection = refreshSubtasksCollection;
            _parentTask = parentTask;


            CurrentSubtask = new Subtask()
            {
                Task_Id_Fk = parentTask.Id
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
            if (SubtaskIsValid())
            {
                AddSubtaskToDatabaseAndShowResult(_subtaskRepository.Add(CurrentSubtask));
            }
        }
        private bool SubtaskIsValid()
        {
            if (SubtaskSubjectIsValid() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Subtask should have a subject.", Icon.InvalidSubtask));
                return false;
            }

            else if (SubtaskDeadlineIsValid() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Invalid Deadline Date", "The selected date is invalid. Cannot create a new Subtask.", Icon.InvalidSubtask));
                return false;
            }

            _parentTask.SubtaskCount += 1;
            return true;
        }
        private bool SubtaskSubjectIsValid()
        {
            return !string.IsNullOrWhiteSpace(CurrentSubtask.Subject);
        }
        private bool SubtaskDeadlineIsValid()
        {
            return CurrentSubtask.Deadline >= DateTime.Now.Date || CurrentSubtask.Deadline is null;
        }
        private void AddSubtaskToDatabaseAndShowResult(bool isAdded)
        {
            if (isAdded)
            {
                _refreshSubtasksCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Add Operation", "Subtask Created Successfully!", Icon.ValidSubtask));
                CloseWindow();
                return;
            }

            _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An Error occured, cannot create the Subtask.", Icon.InvalidSubtask));
        }

        public void CloseWindow()
        {
            Close?.Invoke();
            MainWindowViewModel.Overlay = false;
        }
        #endregion
    }
}
