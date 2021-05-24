using Paraject.Core.Interfaces;
using Paraject.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paraject.Core.Services
{
    class UserCrudService<TEntity> : IMainCrudOperations<TEntity> where TEntity : class
    {
        private readonly string _connectionString;

        public UserCrudService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Add(TEntity entity)
        {
            UserAccount userAccount = entity as UserAccount;
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("spAddUserAccount", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = userAccount.Username;
                    cmd.Parameters.Add("@pasword", SqlDbType.NVarChar, 50).Value = userAccount.Password;
                    cmd.Parameters.Add("@date_created", SqlDbType.DateTime2).Value = DateTime.Now;

                    int NoOfRowsAffected = cmd.ExecuteNonQuery();
                    isAdded = NoOfRowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return isAdded;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(TEntity entity)
        {
            UserAccount userAccount = entity as UserAccount;
            bool isUpdated = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("spUpdateUserAccount", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userAccount.Id;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = userAccount.Username;
                    cmd.Parameters.Add("@pasword", SqlDbType.NVarChar, 50).Value = userAccount.Password;

                    int NoOfRowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = NoOfRowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return isUpdated;
        }
    }
}
