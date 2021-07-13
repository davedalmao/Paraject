using Paraject.MVVM.Models;
using System.Collections.Generic;

namespace Paraject.Core.Repositories.Interfaces
{
    public interface IProjectIdeaRepository
    {
        public bool Add(ProjectIdea projectIdea, int userId);
        public ProjectIdea Get(int projectIdeaId);
        public IEnumerable<ProjectIdea> GetAll(int userId);
        public bool Update(ProjectIdea projectIdea);
        public bool Delete(int projectIdeaId);
    }
}
