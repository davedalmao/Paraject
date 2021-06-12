using Paraject.MVVM.Models;
using System.Collections.Generic;

namespace Paraject.Core.Repositories.Interfaces
{
    interface IProjectRepository
    {
        public bool Add(Project project, int userId);
        public void AddLogo(int userId);
        public Project Get(int id);
        public IEnumerable<Project> GetAll();
        public IEnumerable<Project> FindAll(ProjectOptions projectOption);
        public bool Update(Project project);
        public bool Delete(int id);

    }
}
