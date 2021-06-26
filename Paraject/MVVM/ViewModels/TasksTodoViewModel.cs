using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class TasksTodoViewModel : BaseViewModel
    {
        private readonly int _projectId;
        public TasksTodoViewModel(int projectId)
        {
            _projectId = projectId;
            CurrentTask = new Task();

            ShowAddTaskModalDialogCommand = new DelegateCommand(ShowAddTaskModalDialog);
            CloseModalCommand = new DelegateCommand(SetTaskDefaultThenCloseModal);
            AddTaskCommand = new DelegateCommand(Add);
        }

        #region Properties
        //Model
        public Task CurrentTask { get; set; }

        //Commands
        public ICommand ShowAddTaskModalDialogCommand { get; }
        public ICommand CloseModalCommand { get; }
        public ICommand AddTaskCommand { get; }
        #endregion

        #region Methods
        private void ShowAddTaskModalDialog()
        {
            //Show overlay from MainWindow
            MainWindowViewModel.Overlay = true;

            AddTaskModalDialog addTaskModalDialog = new();
            addTaskModalDialog.DataContext = this;
            addTaskModalDialog.ShowDialog();
        }
        private void SetTaskDefaultThenCloseModal()
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
        private void Add()
        {
            MessageBox.Show($"Subject: {CurrentTask.Subject} \nType: {CurrentTask.Type} \nDescription: {CurrentTask.Description} \nCategory: {CurrentTask.Category} \nPriority: {CurrentTask.Priority} \nDeadline: {CurrentTask.Deadline}");
        }
        #endregion
    }
}
