using Paraject.Core.Repositories.Interfaces;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Paraject.Core.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string _connectionString;

        public ProjectRepository()
        {
            _connectionString = ConnectionString.config;

        }

        public bool Add(Project project, int userId)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("spAddProject", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@project_name", SqlDbType.NVarChar, 50).Value = project.Name;
                    cmd.Parameters.Add("@project_description", SqlDbType.NVarChar, 500).Value = project.Description;
                    cmd.Parameters.Add("@project_option", SqlDbType.NVarChar, 50).Value = project.Option;
                    cmd.Parameters.Add("@project_status", SqlDbType.NVarChar, 12).Value = project.Status;
                    cmd.Parameters.Add("@project_deadline", SqlDbType.DateTime2).Value = project.Deadline;
                    cmd.Parameters.Add("@date_created", SqlDbType.DateTime2).Value = DateTime.Now;

                    int NoOfRowsAffected = cmd.ExecuteNonQuery();
                    isAdded = NoOfRowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)// Violation of unique constraint (Name should be unique)
                    {
                        MessageBox.Show($"{project.Name} Already Exist !!!");
                    }
                    else
                    {
                        MessageBox.Show($"An SQL error occured while processing data. \nError: { ex.Message }");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return isAdded;
        }
        public Project Get(int id)
        {
            throw new System.NotImplementedException();
        }
        public IEnumerable<Project> GetAll()
        {
            throw new System.NotImplementedException();
        }
        public IEnumerable<Project> FindAll(string projectOption)
        {
            throw new System.NotImplementedException();
        }
        public bool Update(Project project)
        {
            throw new System.NotImplementedException();
        }
        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
