using Paraject.MVVM.ViewModels;
using System.Windows.Controls;

namespace Paraject.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ProjectsView.xaml
    /// </summary>
    public partial class ProjectsView : UserControl
    {
        readonly ProjectsViewModel _viewModel;
        public ProjectsView()
        {
            InitializeComponent();
            _viewModel = new();
            DataContext = _viewModel;
        }
    }
}
