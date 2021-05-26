using Paraject.MVVM.Models;

namespace Paraject.Core.Repositories.Interfaces
{
    interface IUserAccountRepository
    {
        public UserAccount Get(int id);
        public bool Add(UserAccount entity);
        public bool Update(UserAccount entity);
        public bool Delete(int id);
    }
}
