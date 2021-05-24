using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraject.Core.Interfaces
{
    public interface IDisplayAll<TEntity>
    {
        public IEnumerable<TEntity> DisplayAll();
    }
}
