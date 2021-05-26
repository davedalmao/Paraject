using Paraject.Core.Commands;

namespace Paraject.MVVM.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region Commands
        public NavigationCommand DashboardViewCommand { get; set; }
        public NavigationCommand ProjectsViewCommand { get; set; }
        public NavigationCommand ProfileViewCommand { get; set; }
        public NavigationCommand ProjectIdeasViewCommand { get; set; }
        public NavigationCommand OptionsViewCommand { get; set; }
        #endregion

        #region ViewModels
        public DashboardViewModel DashboardVM { get; set; }
        public ProjectsViewModel ProjectsVM { get; set; }
        public UserAccountViewModel ProfileVM { get; set; }
        public ProjectIdeasViewModel ProjectIdeasVM { get; set; }
        public OptionsViewModel OptionsVM { get; set; }
        #endregion

        public object CurrentView { get; set; }

        public MainViewModel()
        {
            DashboardVM = new DashboardViewModel();
            ProjectsVM = new ProjectsViewModel();
            ProfileVM = new UserAccountViewModel();
            ProjectIdeasVM = new ProjectIdeasViewModel();
            OptionsVM = new OptionsViewModel();

            CurrentView = DashboardVM;

            DashboardViewCommand = new NavigationCommand(o => { CurrentView = DashboardVM; });

            ProjectsViewCommand = new NavigationCommand(o => { CurrentView = ProjectsVM; });

            ProfileViewCommand = new NavigationCommand(o => { CurrentView = ProfileVM; });

            ProjectIdeasViewCommand = new NavigationCommand(o => { CurrentView = ProjectIdeasVM; });

            OptionsViewCommand = new NavigationCommand(o => { CurrentView = OptionsVM; });
        }
    }
}
