using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class AddTaskModalDialogViewModel : BaseViewModel
    {
        private readonly TaskRepository _taskRepository;
        private readonly Action _refreshTaskCollection;
        private readonly int _currentProjectId;

        public AddTaskModalDialogViewModel(Action refreshTaskCollection, int currentProjectId, string taskType)
        {
            _taskRepository = new TaskRepository();
            _refreshTaskCollection = refreshTaskCollection;
            _currentProjectId = currentProjectId;

            CurrentTask = new Task()
            {
                Type = taskType
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
        private void Add()
        {
            if (!string.IsNullOrWhiteSpace(CurrentTask.Subject))
            {
                bool isAdded = _taskRepository.Add(CurrentTask, _currentProjectId);
                AddOperationResult(isAdded);
            }

            else
            {
                MessageBox.Show("A task should have a subject");
            }
        }
        private void AddOperationResult(bool isAdded)
        {
            if (isAdded)
            {
                _refreshTaskCollection();
                MessageBox.Show("Task Created");
                CloseModal();
            }

            else
            {
                MessageBox.Show("Error occured, cannot create task");
            }
        }
        private void SetCurrentTaskDefaultValues()
        {
            CurrentTask = null;
            CurrentTask = new Task();
        }
        #endregion
    }
}
