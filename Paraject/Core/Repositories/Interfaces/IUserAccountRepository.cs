using Paraject.MVVM.Models;

namespace Paraject.Core.Repositories.Interfaces
{
    interface IUserAccountRepository
    {
        public UserAccount GetByUsername(string username);
        public UserAccount GetById(int id);
        public bool AccountExistsInDatabase(UserAccount userAccount);
        public bool IdExistsInDatabase(int id);
        public bool Add(UserAccount userAccount);
        public bool Update(UserAccount userAccount);
        public bool Delete(int id);
    }
}
