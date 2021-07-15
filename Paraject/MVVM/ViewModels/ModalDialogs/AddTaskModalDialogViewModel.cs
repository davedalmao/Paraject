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

        public AddTaskModalDialogViewModel(Action refreshTaskCollection, int currentProjectId, string taskType)
        {
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
