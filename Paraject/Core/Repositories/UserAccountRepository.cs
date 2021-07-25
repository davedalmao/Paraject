using Paraject.Core.Enums;
using Paraject.Core.Repositories.Interfaces;
using Paraject.Core.Services.DialogService;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Paraject.Core.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly IDialogService _dialogService;
        private readonly string _connectionString;

        public UserAccountRepository()
        {
            _dialogService = new DialogService();
            _connectionString = ConnectionString.config;
        }

        public bool Add(UserAccount userAccount)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("UserAccount.spAddUserAccount", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = userAccount.Username;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = userAccount.Password;
                    cmd.Parameters.Add("@date_created", SqlDbType.DateTime2).Value = DateTime.Now;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isAdded = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)// Violation of unique constraint (Username should be unique)
                    {
                        _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"User: {userAccount.Username} Already Exist!!!", Icon.InvalidUser));
                    }
                    else
                    {
                        _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                                 $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
                    }
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
                }
            }
            return isAdded;
        }
        public UserAccount GetById(int id)
        {
            UserAccount userAccount = null;

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("UserAccount.spGetUserAccountById", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = id;

                var sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    //Move to the first record.  If no records, get out.
                    if (!sqlDataReader.Read()) return null;

                    //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                    int userAccountId = sqlDataReader.GetOrdinal("user_id");
                    int usernameFromDb = sqlDataReader.GetOrdinal("username");
                    int password = sqlDataReader.GetOrdinal("password");
                    int dateCreated = sqlDataReader.GetOrdinal("date_created");

                    //Reads a single UserAccount
                    //Remember, we're already on the first record, so use do/while here.
                    do
                    {
                        userAccount = new UserAccount
                        {
                            Id = sqlDataReader.GetInt32(userAccountId),
                            Username = sqlDataReader.GetString(usernameFromDb),
                            Password = sqlDataReader.GetString(password),
                            DateCreated = sqlDataReader.GetDateTime(dateCreated)
                        };
                    }
                    while (sqlDataReader.Read());
                }
                sqlDataReader.Close();
            }
            catch (SqlException ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                                $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
            }
            catch (Exception ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
            }

            return userAccount;
        }
        public UserAccount GetByUsername(string username)
        {
            UserAccount userAccount = null;

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("UserAccount.spGetUserAccountByName", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username;

                var sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    //Move to the first record.  If no records, get out.
                    if (!sqlDataReader.Read()) return null;

                    //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                    int userAccountId = sqlDataReader.GetOrdinal("user_id");
                    int usernameFromDb = sqlDataReader.GetOrdinal("username");
                    int password = sqlDataReader.GetOrdinal("password");
                    int dateCreated = sqlDataReader.GetOrdinal("date_created");
                    int userAccountImage = sqlDataReader.GetOrdinal("user_account_image");

                    //Reads a single UserAccount
                    //Remember, we're already on the first record, so use do/while here.
                    do
                    {
                        userAccount = new UserAccount
                        {
                            Id = sqlDataReader.GetInt32(userAccountId),
                            Username = sqlDataReader.GetString(usernameFromDb),
                            Password = sqlDataReader.GetString(password),
                            DateCreated = sqlDataReader.GetDateTime(dateCreated)
                        };
                    }
                    while (sqlDataReader.Read());
                }
                sqlDataReader.Close();
            }
            catch (SqlException ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                                $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
            }
            catch (Exception ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
            }

            return userAccount;
        }
        public bool Update(UserAccount userAccount)
        {
            bool isUpdated = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("UserAccount.spUpdateUserAccount", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userAccount.Id;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = userAccount.Username;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = userAccount.Password;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)// Violation of unique constraint (Username should be unique)
                    {
                        _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"User: {userAccount.Username} Already Exist!!!", Icon.InvalidUser));
                    }
                    else
                    {
                        _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                                 $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
                    }
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
                }
            }
            return isUpdated;
        }
        public bool Delete(int id)
        {
            bool isDeleted = false;

            if (id <= 0) { return false; }

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("UserAccount.spDeleteUserAccount", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = id;

                int rowsAffected = cmd.ExecuteNonQuery();
                isDeleted = rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                                $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
            }
            catch (Exception ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
            }

            return isDeleted;
        }
        public bool AccountExistsInDatabase(UserAccount userAccount)
        {
            bool userExists = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("UserAccount.spLoginUserAccount", con))
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
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                                    $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
                }
            }

            return userExists;
        }
        public bool IdExistsInDatabase(int id)
        {
            bool idExists = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("UserAccount.spCheckUserAccountId", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = id;

                    idExists = cmd.ExecuteScalar() is not null;
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                                    $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidUser));
                }
            }

            return idExists;
        }
    }
}
