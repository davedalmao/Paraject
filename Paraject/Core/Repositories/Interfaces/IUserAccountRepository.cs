using Paraject.MVVM.Models;

namespace Paraject.Core.Repositories.Interfaces
{
    interface IUserAccountRepository
    {
        public UserAccount Get(int id);
        public bool Add(UserAccount userAccount);
        public bool Update(UserAccount userAccount);
        public bool Delete(int id);
    }
}
