using Paraject.Core.Repositories.Interfaces;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Paraject.Core.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly string _connectionString;

        public UserAccountRepository()
        {
            _connectionString = ConnectionString.config;
        }

        public bool Add(UserAccount userAccount)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("spAddUserAccount", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = userAccount.Username;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = userAccount.Password;
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
        public UserAccount Get(int id)
        {
            UserAccount userAccount = null;

            if (id != 0)
            {
                using SqlConnection con = new(_connectionString);
                using SqlCommand cmd = new("spGetUserAccount", con);
                try
                {
                    con.Open();
                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = id;

                    var sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        //Reads a single UserAccount
                        sqlDataReader.Read();
                        userAccount = new UserAccount
                        {
                            Id = sqlDataReader.GetInt32(0),
                            Username = sqlDataReader.GetString(1),
                            Password = sqlDataReader.GetString(2),
                            DateCreated = sqlDataReader.GetDateTime(3)
                        };
                    }
                    sqlDataReader.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return userAccount;
        }
        public bool Update(UserAccount userAccount)
        {
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
        public bool Delete(int id)
        {
            bool isDeleted = false;
            if (id != 0)
            {
                using SqlConnection con = new(_connectionString);
                using SqlCommand cmd = new("spDeleteUserAccount", con);
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = id;

                    int NoOfRowsAffected = cmd.ExecuteNonQuery();
                    isDeleted = NoOfRowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return isDeleted;
        }
        public bool AccountExistsInDatabase(UserAccount userAccount)
        {
            bool userExists = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("spLoginUserAccount", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = userAccount.Username;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = userAccount.Password;

                    userExists = cmd.ExecuteScalar() is not null;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return userExists;
        }
    }
}
