using Paraject.MVVM.Models;
using System.Collections.Generic;

namespace Paraject.Core.Repositories.Interfaces
{
    interface IProjectRepository
    {
        public bool Add(Project project, int userId);
        public Project Get(int userId);
        public IEnumerable<Project> GetAll(int userId);
        public IEnumerable<Project> FindAll(int userId, ProjectOptions projectOption);
        public bool Update(Project project);
        public bool Delete(int userId);

    }
}
