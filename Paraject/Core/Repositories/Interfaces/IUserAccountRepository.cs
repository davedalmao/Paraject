using Paraject.MVVM.Models;

namespace Paraject.Core.Repositories.Interfaces
{
    interface IUserAccountRepository
    {
        public UserAccount Get(string username);
        public bool AccountExistsInDatabase(UserAccount userAccount);
        public bool Add(UserAccount userAccount);
        public bool Update(UserAccount userAccount);
        public bool Delete(int id);
    }
}
