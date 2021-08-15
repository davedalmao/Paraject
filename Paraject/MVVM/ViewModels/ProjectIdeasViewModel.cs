using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.ModalDialogs;
using Paraject.MVVM.ViewModels.Windows;
using Paraject.MVVM.Views.ModalDialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectIdeasViewModel : BaseViewModel
    {
        private readonly int _currentUserId;
        private readonly ProjectIdeaRepository _projectIdeaRepository;

        public ProjectIdeasViewModel(int currentUserId)
        {
            _projectIdeaRepository = new ProjectIdeaRepository();
            _currentUserId = currentUserId;

            ShowAddProjectIdeaModalDialogCommand = new ParameterizedDelegateCommand(ShowAddProjectIdeaModalDialog);
            ShowProjectIdeaDetailsModalDialogCommand = new ParameterizedDelegateCommand(ShowProjectIdeaDetailsModalDialog);

            DisplayProjectIdeas();
        }

        #region Properties
        public ObservableCollection<ProjectIdea> ProjectIdeas { get; set; }
        public ObservableCollection<GridTileData> ProjectIdeaCardsGrid { get; set; }

        public ICommand ShowAddProjectIdeaModalDialogCommand { get; }
        public ICommand ShowProjectIdeaDetailsModalDialogCommand { get; }
        #endregion

        #region Methods
        private void DisplayProjectIdeas()
        {
            GetValuesForProjectIdeasCollection();
            SetNewGridDisplay();
            TaskCardGridLocation();
        }
        private void GetValuesForProjectIdeasCollection()
        {
            ProjectIdeas = null;
            ProjectIdeas = new ObservableCollection<ProjectIdea>(_projectIdeaRepository.GetAll(_currentUserId));
        }
        private void SetNewGridDisplay()
        {
            ProjectIdeaCardsGrid = null;
            ProjectIdeaCardsGrid = new ObservableCollection<GridTileData>();
        }
        private void TaskCardGridLocation()
        {
            int row = -1;
            int column = -1;

            //This is for a 3 column grid, with n number of rows
            for (int i = 0; i < ProjectIdeas.Count; i++)
            {
                if (column == 2)
                {
                    column = 0;
                }

                else
                {
                    column++;
                }

                if (i % 3 == 0)
                {
                    row++;
                }

                GridTileData td = new(ProjectIdeas[i], row, column);
                ProjectIdeaCardsGrid.Add(td);
            }
        }

        private void ShowAddProjectIdeaModalDialog(object owner)
        {
            MainWindowViewModel.Overlay = true;

            AddProjectIdeaModalDialog addProjectIdeaModalDialog = new()
            {
                DataContext = new AddProjectIdeaModalDialogViewModel(DisplayProjectIdeas, _currentUserId),
                Owner = owner as Window
            };
            addProjectIdeaModalDialog.ShowDialog();
        }
        private void ShowProjectIdeaDetailsModalDialog(object projectIdeaId)
        {
            MainWindowViewModel.Overlay = true;

            int selectedProjectIdea = (int)projectIdeaId;
            ProjectIdeaDetailsModalDialogViewModel projectIdeaDetailsModalDialogViewModel = new(DisplayProjectIdeas, selectedProjectIdea);

            ProjectIdeaDetailsModalDialog projectIdeaDetailsModalDialog = new();
            projectIdeaDetailsModalDialog.DataContext = projectIdeaDetailsModalDialogViewModel;
            projectIdeaDetailsModalDialog.ShowDialog();
        }
        #endregion
    }
}
