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
    public class AddTaskModalDialogViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly TaskRepository _taskRepository;
        private readonly Action _refreshTaskCollection;

        public AddTaskModalDialogViewModel(Action refreshTaskCollection, int currentProjectId, string taskType)
        {
            _dialogService = new DialogService();
            _taskRepository = new TaskRepository();
            _refreshTaskCollection = refreshTaskCollection;

            CurrentTask = new Task()
            {
                Type = taskType,
                Project_Id_Fk = currentProjectId
            };
            CurrentTaskType = taskType.Replace("_", " ");

            CloseModalCommand = new DelegateCommand(CloseModal);
            AddTaskCommand = new DelegateCommand(Add);
        }

        #region Properties
        public Task CurrentTask { get; set; }
        public string CurrentTaskType { get; set; }

        public ICommand AddTaskCommand { get; }
        public ICommand CloseModalCommand { get; }
        #endregion

        #region Methods
        private void Add()
        {
            if (TaskIsValid())
            {
                AddTaskToDatabaseAndShowResult(_taskRepository.Add(CurrentTask));
            }
        }
        private bool TaskIsValid()
        {
            if (TaskSubjecIsValid() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Task should have a subject.", Icon.InvalidTask));
                return false;
            }

            else if (TaskDeadlineDateIsValid() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Invalid Deadline Date", "The selected date is invalid. Cannot create a new Task.", Icon.InvalidTask));
                return false;
            }

            return true;
        }
        private bool TaskSubjecIsValid()
        {
            return !string.IsNullOrWhiteSpace(CurrentTask.Subject);
        }
        private bool TaskDeadlineDateIsValid()
        {
            return CurrentTask.Deadline >= DateTime.Now.Date || CurrentTask.Deadline is null;
        }
        private void AddTaskToDatabaseAndShowResult(bool isValid)
        {
            if (isValid)
            {
                _refreshTaskCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Add Operation", "Task Created Successfully!", Icon.ValidTask));
                CloseModal();
                return;
            }

            _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An Error occured, cannot create the Task.", Icon.InvalidTask));
        }

        private void SetCurrentTaskDefaultValues()
        {
            CurrentTask = null;
            CurrentTask = new Task();
        }
        private void CloseModal()
        {
            MainWindowViewModel.Overlay = false;

            SetCurrentTaskDefaultValues();

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
