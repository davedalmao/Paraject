
using Paraject.Core.Enums;
using Paraject.MVVM.Models;
using System.Collections.Generic;

namespace Paraject.Core.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        public bool Add(Task task, int projectId);
        public Task Get(int taskId);
        public IEnumerable<Task> FindAll(int projectId, Status taskStatus, Priority taskPriority, Category taskCategory);
        public bool Update(Task task);
        public bool Delete(int taskId);
    }
}
