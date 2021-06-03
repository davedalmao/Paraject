using Paraject.Core.Commands;
using Paraject.Core.Repositories;
using Paraject.MVVM.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels.Windows
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region UserAccount elements
        private readonly UserAccountRepository _userAccountRepository;
        public ICommand UpdateCurrentUserCommand { get; }
        public ICommand DeleteCurrentUserCommand { get; }
        #endregion

        #region Commands To Navigate From Different Views
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
        public UserAccount CurrentUserAccount { get; set; }

        public MainWindowViewModel(UserAccount currentUserAccount)
        {
            CurrentUserAccount = currentUserAccount;
            _userAccountRepository = new UserAccountRepository();

            UpdateCurrentUserCommand = new DelegateCommand(Update);
            DeleteCurrentUserCommand = new DelegateCommand(Delete);


            //pass currentUserAccount to the ViewModels that need to access the User's details
            DashboardVM = new DashboardViewModel();
            ProjectsVM = new ProjectsViewModel();
            ProfileVM = new UserAccountViewModel();
            //ProfileVM = new UserAccountViewModel(currentUserAccount.Username);
            ProjectIdeasVM = new ProjectIdeasViewModel();
            OptionsVM = new OptionsViewModel();

            CurrentView = DashboardVM;

            DashboardViewCommand = new NavigationCommand(o => { CurrentView = DashboardVM; });
            ProjectsViewCommand = new NavigationCommand(o => { CurrentView = ProjectsVM; });
            ProfileViewCommand = new NavigationCommand(o => { CurrentView = ProfileVM; });
            ProjectIdeasViewCommand = new NavigationCommand(o => { CurrentView = ProjectIdeasVM; });
            OptionsViewCommand = new NavigationCommand(o => { CurrentView = OptionsVM; });

            Get();
        }

        public void Get()
        {
            UserAccount userAccount = _userAccountRepository.Get(CurrentUserAccount.Username);

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
                    UpdateCurrentUser();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void UpdateCurrentUser()
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

                MessageBox.Show($"Id: {CurrentUserAccount.Id} \nUsername: {CurrentUserAccount.Username} \nPassword: {CurrentUserAccount.Password} \nDate Created: {CurrentUserAccount.DateCreated}");
                //bool isDeleted = _userAccountRepository.Delete(CurrentUserAccount.Id);

                //if (isDeleted)
                //{
                //    MessageBox.Show($"Your account {CurrentUserAccount.Username} is now deleted");
                //}
                //after deleting account, popup a message box, then redirect to login page
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
