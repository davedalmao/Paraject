using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraject.Core.Interfaces
{
    public interface IMainCrudOperations<TEntity>
    {
        public TEntity Get(int id);
        public bool Add(TEntity entity);
        public bool Update(TEntity entity);
        public bool Delete(int id);
    }
}
