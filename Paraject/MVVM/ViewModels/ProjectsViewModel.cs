using System.Windows.Input;

namespace Paraject.MVVM.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        public object CurrentView { get; set; }
        public ICommand DisplayProjectsViewCommand { get; }


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
