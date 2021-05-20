using Paraject.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraject.MVVM.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public NavigationCommand DashboardViewCommand { get; set; }
        public NavigationCommand ProjectsViewCommand { get; set; }


        public DashboardViewModel DashboardVM { get; set; }
        public ProjectsViewModel ProjectsVM { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            DashboardVM = new DashboardViewModel();
            ProjectsVM = new ProjectsViewModel();

            CurrentView = DashboardVM;

            DashboardViewCommand = new NavigationCommand(o =>
            {
                CurrentView = DashboardVM;
            });

            ProjectsViewCommand = new NavigationCommand(o =>
            {
                CurrentView = ProjectsVM;
            });
        }
    }
}
