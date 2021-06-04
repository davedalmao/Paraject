using Paraject.MVVM.Models;
using System.Collections.Generic;

namespace Paraject.Core.Repositories.Interfaces
{
    interface IProjectRepository
    {
        public bool Add(Project project);
        public Project Get(int id);
        public IEnumerable<Project> GetAll();
        public IEnumerable<Project> FindAll(string projectOption);
        public bool Update(Project project);
        public bool Delete(int id);

    }
}
