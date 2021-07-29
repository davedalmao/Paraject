using Paraject.Core.Commands;
using Paraject.Core.Enums;
using Paraject.Core.Repositories;
using Paraject.Core.Services.DialogService;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TaskDetailsViewModel : BaseViewModel
    {
        private readonly Action _refreshTaskCollection;
        private readonly ProjectContentViewModel _projectContentViewModel;

        private readonly IDialogService _dialogService;
        private readonly TaskRepository _taskRepository;
        private readonly ProjectRepository _projectRepository;

        private readonly string _unmodifiedSelectedTaskStatus;
        private readonly string _unmodifiedParentProjectStatus;

        public TaskDetailsViewModel(Action refreshTaskCollection, ProjectContentViewModel projectContentViewModel, Task selectedTask, Project parentProject)
        {
            _refreshTaskCollection = refreshTaskCollection;
            _projectContentViewModel = projectContentViewModel;

            _dialogService = new DialogService();
            _taskRepository = new TaskRepository();
            _projectRepository = new ProjectRepository();

            SelectedTask = selectedTask;
            _unmodifiedSelectedTaskStatus = SelectedTask.Status;

            ParentProject = parentProject;

            /* I have to GET the Parent Project's Status property here (instead of getting it's Status property through the Project object that is passed in the constructor),
               because if the Project object's Status property is modified (without being UPDATED through a repository),
               then the Parent Project's Status (that will be passed here) breaks data integrity, therefore producing unexpected results */
            _unmodifiedParentProjectStatus = _projectRepository.Get(parentProject.Id).Status;

            UpdateTaskCommand = new DelegateCommand(Update);
            DeleteTaskCommand = new DelegateCommand(Delete);
        }

        #region Properties
        public Project ParentProject { get; set; }
        public Task SelectedTask { get; set; }

        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        #endregion

        #region Methods
        private void Update()
        {
            if (TaskIsValid())
            {
                UpdateTaskAndShowResult(_taskRepository.Update(SelectedTask));
            }
        }
        private bool TaskIsValid()
        {
            if (TaskSubjectIsValid() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Incorrect Data Entry", "A Task should have a subject.", Icon.InvalidTask));
                return false;
            }

            else if (TaskDeadlineDateIsValid() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Invalid Deadline Date", $"The selected date is invalid. Cannot update this Task. \n\nThe deadline date should be within: \n{ParentProject.DateCreated:MMMM dd, yyyy} (Parent Project's Created Date) \nto \n{ParentProject.Deadline:MMMM dd, yyyy} (Parent Project's Deadline) \n\nOr not have a deadline for this task at all.", Icon.InvalidTask));
                return false;
            }

            else if (TaskStatusCanBeCompleted() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", $"Unable to change this Task's Status to \"Completed\" because there are still {SelectedTask.SubtaskCount} unfinished subtask/s remaining.", Icon.InvalidTask));
                return false;
            }

            else if (TaskStatusCanBeChangedFromCompletedToOtherStatus() == false)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", $"Unable to change this Task's status as \"{SelectedTask.Status.Replace("_", " ")}\" because the Project's Status that this task belongs to is now completed. \n\nChange parent Project's status to \"Open\" or \"In Progress\" to change this task's status.", Icon.InvalidTask));
                return false;
            }

            UpdateTaskCount();
            return true;
        }
        private bool TaskSubjectIsValid()
        {
            return !string.IsNullOrWhiteSpace(SelectedTask.Subject);
        }
        private bool TaskDeadlineDateIsValid()
        {
            if (ParentProject.Deadline is not null)
            {
                return (SelectedTask.Deadline <= ParentProject.Deadline && SelectedTask.Deadline >= ParentProject.DateCreated.Date) || SelectedTask.Deadline is null;
            }

            return SelectedTask.Deadline >= ParentProject.DateCreated.Date || SelectedTask.Deadline is null;
        }
        private bool TaskStatusCanBeCompleted()
        {
            //A Task's status can only be changed as "Completed" if they don't have any unfinished Subtasks
            if (SelectedTask.Status == "Completed")
            {
                return SelectedTask.SubtaskCount == 0;
            }

            //A Task's status can only be changed if the parent Project's status is not Completed (either Open or In Progress)
            return true;
        }
        private bool TaskStatusCanBeChangedFromCompletedToOtherStatus()
        {
            if (_unmodifiedParentProjectStatus != "Completed")
            {
                return true;
            }

            return SelectedTask.Status == "Completed"; //The selected task's status should be "Completed" so we can update its other information
        }
        private void UpdateTaskCount()
        {
            //if the Previous SelectedTask's Status is "Completed" and we change the task's status to Open or In Progress, +! to the parent's task count (add 1 more "Incomplete" task)
            if (_unmodifiedSelectedTaskStatus == "Completed")
            {
                IncreaseTaskCountIfTaskIsUnfinished();
            }

            //if the Previous SelectedTask's Status is "Open" or "In Progress", and we change the task's status to Completed, -1 to the parent's task count (minus 1  "Incomplete" task)
            else
            {
                DecreaseTaskCountIfTaskIsCompleted();
            }
        }
        private void IncreaseTaskCountIfTaskIsUnfinished()
        {
            if (SelectedTask.Status != "Completed")
            {
                ParentProject.TaskCount += 1;
            }
        }
        private void DecreaseTaskCountIfTaskIsCompleted()
        {
            if (SelectedTask.Status == "Completed")
            {
                ParentProject.TaskCount -= 1;
            }
        }
        private void UpdateTaskAndShowResult(bool isValid)
        {
            if (isValid)
            {
                _refreshTaskCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Update Operation", "Task Updated Successfully!", Icon.ValidTask));
                return;
            }

            _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot update the Task.", Icon.InvalidTask));
        }

        private void Delete()
        {
            DialogResults result = _dialogService.OpenDialog(new YesNoMessageBoxViewModel("Delete Operation",
                                                             "Do you want to DELETE this task? \n\nAll Subtasks that belongs to this Task will also be deleted.",
                                                             Icon.Task));
            if (result == DialogResults.Yes)
            {
                DeleteProject();
            }
        }
        private void DeleteProject()
        {
            bool isDeleted = _taskRepository.Delete(SelectedTask.Id);
            if (isDeleted)
            {
                ParentProject.TaskCount -= 1;

                _refreshTaskCollection();
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Delete Operation", "Task Deleted Successfully!", Icon.ValidTask));
                MainWindowViewModel.CurrentView = _projectContentViewModel;
            }
            else
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", "An error occured, cannot delete the Task.", Icon.InvalidTask));
            }
        }
        #endregion
    }
}
