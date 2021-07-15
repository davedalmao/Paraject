using Paraject.MVVM.Models;
using System.Collections.Generic;

namespace Paraject.Core.Repositories.Interfaces
{
    public interface INoteRepository
    {
        public bool Add(Note note);
        public Note Get(int noteId);
        public IEnumerable<Note> GetAll(int projectId);
        public bool Update(Note note);
        public bool Delete(int noteId);
    }
}
