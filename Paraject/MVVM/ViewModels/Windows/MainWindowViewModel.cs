using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.Views.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.Windows
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly UserAccountRepository _userAccountRepository;
        private static bool _overlay;
        private static object _currentView;

        #region EventHandlers
        //Event handler for static property (MainWindowOverlay), since PropertyChanged.Fody (nuget package) doesn't notify static property changes
        //The static property name in this ViewModel is Overlay. The event name is therefore OverlayChanged (or else it will not notify the changes)
        public static event EventHandler OverlayChanged;
        public static event EventHandler CurrentViewChanged;
        #endregion

        #region Constructor
        public MainWindowViewModel(UserAccount currentUserAccount)
        {
            CurrentUserAccount = currentUserAccount;
            _userAccountRepository = new UserAccountRepository();

            UpdateCurrentUserCommand = new DelegateCommand(Update);
            DeleteCurrentUserCommand = new DelegateCommand(Delete);
            LogoutCommand = new DelegateCommand(Logout);


            //pass currentUserAccount to the ViewModels that need to access the User's details
            DashboardVM = new DashboardViewModel();
            ProjectsVM = new ProjectsViewModel(currentUserAccount.Id);
            ProfileVM = new UserAccountViewModel();
            ProjectIdeasVM = new ProjectIdeasViewModel(currentUserAccount.Id);
            OptionsVM = new OptionsViewModel();

            CurrentView = ProjectsVM;

            DashboardViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = DashboardVM; });
            ProjectsViewCommand = new DelegateCommand(NavigateToProjectsView);
            ProfileViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = ProfileVM; });
            ProjectIdeasViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = ProjectIdeasVM; });
            OptionsViewCommand = new ParameterizedDelegateCommand(o => { CurrentView = OptionsVM; });

            Get();
        }
        #endregion

        #region Properties
        public UserAccount CurrentUserAccount { get; set; }
        public static bool Overlay
        {
            get { return _overlay; }
            set
            {
                _overlay = value;
                if (OverlayChanged is not null)
                    OverlayChanged(null, EventArgs.Empty);
            }
        }
        public static object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                if (CurrentViewChanged is not null)
                    CurrentViewChanged(null, EventArgs.Empty);
            }
        }

        #region ViewModels (that will navigate with their associated Views)
        public DashboardViewModel DashboardVM { get; set; }
        public ProjectsViewModel ProjectsVM { get; set; }
        public UserAccountViewModel ProfileVM { get; set; }
        public ProjectIdeasViewModel ProjectIdeasVM { get; set; }
        public OptionsViewModel OptionsVM { get; set; }
        #endregion

        #region Commands 
        //Navigation Commands
        public ICommand DashboardViewCommand { get; }
        public ICommand ProjectsViewCommand { get; }
        public ICommand ProfileViewCommand { get; }
        public ICommand ProjectIdeasViewCommand { get; }
        public ICommand OptionsViewCommand { get; }

        // Update/Delete UserAccount Commands
        public ICommand UpdateCurrentUserCommand { get; }
        public ICommand DeleteCurrentUserCommand { get; }

        //MainWindow Command
        public ICommand LogoutCommand { get; }
        #endregion
        #endregion

        #region Navigation Methods
        public void NavigateToProjectsView()
        {
            ProjectsVM = new ProjectsViewModel(CurrentUserAccount.Id);
            CurrentView = ProjectsVM;
        }
        #endregion

        #region Methods Used in UserAccountView
        public void Get()
        {
            UserAccount userAccount = _userAccountRepository.GetByUsername(CurrentUserAccount.Username);

            if (userAccount is not null)
            {
                CurrentUserAccount.Id = userAccount.Id;
                CurrentUserAccount.Username = userAccount.Username;
                CurrentUserAccount.Password = userAccount.Password;
                CurrentUserAccount.DateCreated = userAccount.DateCreated;
            }
            else
            {
                MessageBox.Show("User Account not Found!");
            }
        }
        public void Update()
        {
            try
            {
                bool idExists = _userAccountRepository.IdExistsInDatabase(CurrentUserAccount.Id);
                if (idExists)
                {
                    UpdateUserAccount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void UpdateUserAccount()
        {
            bool isUpdate = _userAccountRepository.Update(CurrentUserAccount);

            if (isUpdate)
            {
                MessageBox.Show("User Updated");
            }
            else
            {
                MessageBox.Show("Failed to Update User");

            }
        }
        public void Delete()
        {
            try
            {
                MessageBoxResult Result = MessageBox.Show("Do you want to DELETE your account?", "Delete Operation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    DeleteUserAccount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DeleteUserAccount()
        {
            bool isDeleted = _userAccountRepository.Delete(CurrentUserAccount.Id);

            if (isDeleted)
            {
                MessageBox.Show($"Your account {CurrentUserAccount.Username} is now deleted");
                ShowLoginWindow();
            }

            else
            {
                MessageBox.Show("An error occured when deleting your account, please try again");
            }
        }
        #endregion

        #region Window Methods
        public void Logout()
        {
            MessageBoxResult Result = MessageBox.Show("Do you want Logout?", "Logout Account", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                ShowLoginWindow();
            }
        }
        public void CloseMainWindow()
        {
            if (CloseWindow.WinObject != null)
                CloseWindow.CloseParent();
        }
        private void ShowLoginWindow()
        {
            LoginWindow loginWindow = new();
            loginWindow.Show();
            CloseMainWindow();
        }
        #endregion
    }
}
