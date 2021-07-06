using Paraject.Core.Commands;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class AddSubtaskModalDialogViewModel : BaseViewModel
    {
        public AddSubtaskModalDialogViewModel()
        {
            CurrentSubtask = new Subtask();
            AddSubtaskCommand = new DelegateCommand(Add);
            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);
        }



        public Subtask CurrentSubtask { get; set; }
        public ICommand CloseModalDialogCommand { get; }
        public ICommand AddSubtaskCommand { get; }

        private void Add()
        {
            MessageBox.Show($"Subject: {CurrentSubtask.Subject} \nPriority: {CurrentSubtask.Priority} \nDeadline: {CurrentSubtask.Deadline} \nDescription: {CurrentSubtask.Description}");
        }
        private void CloseModalDialog()
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
    }
}
