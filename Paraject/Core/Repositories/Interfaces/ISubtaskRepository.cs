using Paraject.MVVM.Models;
using System.Collections.Generic;

namespace Paraject.Core.Repositories.Interfaces
{
    public interface ISubtaskRepository
    {
        public bool Add(Subtask subtask);
        public Subtask Get(int subtaskId);
        public IEnumerable<Subtask> GetAll(int taskId);
        public IEnumerable<Subtask> FindAll(int taskId, string subtaskStatus, string subtaskPriority);
        public bool Update(Subtask subtask);
        public bool Delete(int subtaskId);
    }
}
