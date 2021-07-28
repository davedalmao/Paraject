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
        private readonly Action _refreshTaskCollection;
        private readonly TaskRepository _taskRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly string _unmodifiedParentProjectStatus;


        public AddTaskModalDialogViewModel(Action refreshTaskCollection, Project parentProject, string taskType)
        {
            _dialogService = new DialogService();
            _refreshTaskCollection = refreshTaskCollection;

            _taskRepository = new TaskRepository();
            _projectRepository = new ProjectRepository();

            ParentProject = parentProject;

            /* I have to GET the Parent Project's Status property here (instead of getting it's Status property through the Project object that is passed in the constructor),
               because if the Project object's Status property is modified (without being UPDATED through a repository),
               then the Parent Project's Status (that will be passed here) breaks data integrity, therefore producing unexpected results */
            _unmodifiedParentProjectStatus = _projectRepository.Get(parentProject.Id).Status;

            SelectedTask = new Task()
            {
                Type = taskType,
                Project_Id_Fk = parentProject.Id
            };
            SelectedTaskType = taskType.Replace("_", " ");

            CloseModalCommand = new DelegateCommand(CloseModal);
            AddTaskCommand = new DelegateCommand(Add);
        }

        #region Properties
        public Project ParentProject { get; set; }
        public Task SelectedTask { get; set; }
        public string SelectedTaskType { get; set; }

        public ICommand AddTaskCommand { get; }
        public ICommand CloseModalCommand { get; }
        #endregion

        #region Methods
        private void Add()
        {
            if (TaskIsValid())
            {
                AddTaskToDatabaseAndShowResult(_taskRepository.Add(SelectedTask));
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
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Invalid Deadline Date", $"The selected date is invalid. Cannot create a new Task. \n\nThe deadline date should be on or after {ParentProject.DateCreated:d} (the parent Project's created date).", Icon.InvalidTask));
                return false;
            }

            else if (_unmodifiedParentProjectStatus == "Completed")
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Add Operation", $"Cannot add a new task for {ParentProject.Name}, change the project's status to \"Open\" or \"In Progress\" to add a new task.", Icon.InvalidTask));
                CloseModal();
                return false;
            }

            ParentProject.TaskCount += 1;
            return true; //A Task is valid if it passes all of the checks above
        }
        private bool TaskSubjecIsValid()
        {
            return !string.IsNullOrWhiteSpace(SelectedTask.Subject);
        }
        private bool TaskDeadlineDateIsValid()
        {
            return SelectedTask.Deadline >= DateTime.Now.Date || SelectedTask.Deadline is null;
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

        private void CloseModal()
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
