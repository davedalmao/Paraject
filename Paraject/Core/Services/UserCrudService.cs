using Paraject.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraject.Core.Services
{
    class UserCrudService<TEntity> : IMainCrudOperations<TEntity>
    {
        private readonly SqlConnection _sqlCon;
        private readonly SqlCommand _sqlCmd;

        public UserCrudService()
        {
            _sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ParajectDbTest"].ConnectionString);
            _sqlCmd = new SqlCommand
            {
                Connection = _sqlCon,
                CommandType = CommandType.StoredProcedure
            };
        }

        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
