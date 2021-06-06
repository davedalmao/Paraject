using Paraject.Core.Commands;
using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        //public object CurrentView { get; set; }

        public ICommand DisplayProjectsViewCommand { get; }
        public ICommand AllProjectsCommand { get; }
        public ICommand PersonalProjectsCommand { get; }
        public ICommand PaidProjectsCommand { get; }

        public string Message { get; set; }

        public DisplayProjectsViewModel DisplayProjectsVM { get; set; }

        public ProjectsViewModel()
        {
            DisplayProjectsVM = new DisplayProjectsViewModel();

            AllProjectsCommand = new DelegateCommand(all);
            PersonalProjectsCommand = new DelegateCommand(personal);
            PaidProjectsCommand = new DelegateCommand(paid);

            all();
        }

        public void all()
        {
            //MessageBox.Show("all");
            Message = "all";
        }
        public void personal()
        {
            //MessageBox.Show("personal");
            Message = "personal";
        }
        public void paid()
        {
            //MessageBox.Show("paid");
            Message = "paid";
        }


        /*
        if (all projects radio button is selected)
        {
            GetAll();
            background = red;
        }
        else if (personal projects radio button is selected)
        {
            FindAll(Personal);
            background = yellow;
        }
        else if (paid projects radio button is selected)
        {
            FindAll(paid);
            background = blue;
        }
        */
    }
}
