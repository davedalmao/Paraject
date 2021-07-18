using Paraject.Core.Commands;
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
            if (!string.IsNullOrWhiteSpace(CurrentTask.Subject))
            {
                bool isAdded = _taskRepository.Add(CurrentTask);
                AddOperationResult(isAdded);
            }

            else
            {
                string iconSource = "/UiDesign/Images/Logo/defaultProjectLogo.png";

                OkayMessageBoxViewModel okayMessageBox = new("Data Entry", "A Task should have a subject", iconSource);
                _dialogService.OpenDialog(okayMessageBox);
            }
        }
        private void AddOperationResult(bool isAdded)
        {
            string iconSource = "/UiDesign/Images/Logo/defaultProjectLogo.png";

            if (isAdded)
            {
                _refreshTaskCollection();

                OkayMessageBoxViewModel okayMessageBox = new("Add Operation", "Task Created Successfully!", iconSource);
                _dialogService.OpenDialog(okayMessageBox);

                CloseModal();
            }

            else
            {
                OkayMessageBoxViewModel okayMessageBox = new("Add Operation", "An Error occured, cannot create the Subtask ;(", iconSource);
                _dialogService.OpenDialog(okayMessageBox);
            }
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
