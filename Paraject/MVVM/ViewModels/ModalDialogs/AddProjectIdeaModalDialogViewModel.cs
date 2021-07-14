using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.ModalDialogs
{
    public class AddProjectIdeaModalDialogViewModel : BaseViewModel
    {
        private readonly ProjectIdeaRepository _projectIdeaRepository;
        private readonly Action _refreshProjectIdeasCollection;

        public AddProjectIdeaModalDialogViewModel(Action refreshProjectIdeasCollection, int currentUserId)
        {
            _projectIdeaRepository = new ProjectIdeaRepository();
            CurrentProjectIdea = new ProjectIdea()
            {
                User_Id_Fk = currentUserId
            };

            _refreshProjectIdeasCollection = refreshProjectIdeasCollection;

            AddProjectIdeaCommand = new DelegateCommand(Add);
            CloseModalDialogCommand = new DelegateCommand(CloseModalDialog);
        }

        #region Properties
        public ProjectIdea CurrentProjectIdea { get; set; }
        public ICommand AddProjectIdeaCommand { get; }
        public ICommand CloseModalDialogCommand { get; }
        #endregion

        #region Methods
        private void Add()
        {
            if (!string.IsNullOrWhiteSpace(CurrentProjectIdea.Name))
            {
                bool isAdded = _projectIdeaRepository.Add(CurrentProjectIdea);
                AddOperationResult(isAdded);
            }

            else
            {
                MessageBox.Show("A subtask should have a subject");
            }
        }
        private void AddOperationResult(bool isAdded)
        {
            if (isAdded)
            {
                //messagebox issue (this is just temporary, we're going to use a custom MessageBox anyway)
                _refreshProjectIdeasCollection();
                MessageBox.Show("Project Idea Added");
                CloseModalDialog();
            }

            else
            {
                MessageBox.Show("Error occured, cannot create project idea");
            }
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
        #endregion
    }
}
