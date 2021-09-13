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
        private readonly Action _refreshSubtasksCollection;
        private readonly SubtaskRepository _subtaskRepository;
        private readonly TaskRepository _taskRepository;
        private RelayCommand _closeCommand;
        private readonly string _unmodifiedParentTaskStatus;
        private readonly DateTime? _unmodifiedParentTaskDeadline;


        public AddSubtaskModalDialogViewModel(Task parentTask, Action refreshSubtasksCollection)
        {
            _dialogService = new DialogService();
            _refreshSubtasksCollection = refreshSubtasksCollection;

            _subtaskRepository = new SubtaskRepository();
            _taskRepository = new TaskRepository();


            ParentTask = parentTask;

            /* I have to GET the Parent Task's properties here (instead of getting it's properties through the Task object that is passed in the constructor),
               because if the Task object's properties are modified (without being UPDATED through a repository),
               then the Parent Task's properties (that will be passed here) breaks data integrity, therefore producing unexpected results */
            _unmodifiedParentTaskStatus = _taskRepository.Get(parentTask.Id).Status;
            _unmodifiedParentTaskDeadline = _taskRepository.Get(parentTask.Id).Deadline;

            CurrentSubtask = new Subtask()
            {
                Task_Id_Fk = parentTask.Id
            };

            AddSubtaskCommand = new RelayCommand(Add);
        }

        #region Properties
        public Task ParentTask { get; set; }
        public Subtask CurrentSubtask { get; set; }
        public Action Close { get; set; }

        public ICommand AddSubtaskCommand { get; }
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(CloseWindow);
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
                return SubtaskDeadlineDateResult();
            }

            else if (_unmodifiedParentTaskStatus == "Completed")
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Add Operation", $"Cannot add a new subtask for this task, change the task's status to \"Open\" or \"In Progress\" to add a new subtask.", Icon.InvalidSubtask));
                CloseWindow();
                return false;
            }

            ParentTask.SubtaskCount += 1;
            return true; //A Subtask is valid if it passes all of the checks above
        }
        private bool SubtaskSubjectIsValid()
        {
            return !string.IsNullOrWhiteSpace(CurrentSubtask.Subject);
        }
        private bool SubtaskDeadlineIsValid()
        {
            if (_unmodifiedParentTaskDeadline is not null)
            {
                return (CurrentSubtask.Deadline <= _unmodifiedParentTaskDeadline && CurrentSubtask.Deadline >= ParentTask.DateCreated.Date) || CurrentSubtask.Deadline is null;
            }

            return CurrentSubtask.Deadline >= DateTime.Now.Date || CurrentSubtask.Deadline is null || CurrentSubtask.Deadline >= ParentTask.DateCreated.Date;
        }
        private bool SubtaskDeadlineDateResult()
        {
            if (_unmodifiedParentTaskDeadline is not null)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Invalid Deadline Date", $"The selected date is invalid. Cannot create a new Subtask. \n\nThe deadline date should be within: \n{ParentTask.DateCreated:MMMM dd, yyyy} (Parent Task's Created Date) \nto \n{_unmodifiedParentTaskDeadline:MMMM dd, yyyy} (Parent Task's Deadline) \n\nOr not have a deadline for this subtask at all.", Icon.InvalidSubtask));
                return false;
            }

            _dialogService.OpenDialog(new OkayMessageBoxViewModel("Invalid Deadline Date", $"The selected date is invalid. Cannot create a new Subtask. \n\nThe deadline date should be on or after {ParentTask.DateCreated:MMMM dd, yyyy} (the Parent Task's created date).", Icon.InvalidSubtask));
            return false;
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
