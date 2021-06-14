using Paraject.Core.Converters;
using Paraject.Core.Enums;
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
    public enum ProjectOptions
    {
        Personal,
        Paid
    }

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
            using (SqlCommand cmd = new("Project.spAddProject", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@project_name", SqlDbType.NVarChar, 50).Value = project.Name;
                    cmd.Parameters.Add("@project_description", SqlDbType.NVarChar, 500).Value = project.Description;
                    cmd.Parameters.Add("@project_option", SqlDbType.NVarChar, 50).Value = project.Option;
                    cmd.Parameters.Add("@project_status", SqlDbType.NVarChar, 12).Value = Enum.GetName(Status.Open);
                    cmd.Parameters.Add("@project_deadline", SqlDbType.DateTime2).Value = project.Deadline;
                    cmd.Parameters.Add("@project_logo", SqlDbType.VarBinary).Value = ImageOperations.ImageToBytes(project.Logo);
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
        public Project Get(int userId)
        {
            Project project = null;

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("Project.spGetProject", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = userId;

                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    //Reads a single Project
                    sqlDataReader.Read();
                    project = new Project()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        User_Id_Fk = sqlDataReader.GetInt32(1),
                        Name = sqlDataReader.GetString(2),
                        Description = sqlDataReader.GetString(3),
                        Option = sqlDataReader.GetString(4),
                        Status = sqlDataReader.GetString(5),
                        Deadline = sqlDataReader.GetString(6),
                        DateCreated = sqlDataReader.GetDateTime(7)
                    };
                }
                sqlDataReader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return project;
        }
        public IEnumerable<Project> GetAll(int userId)
        {
            List<Project> projects = new();

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Project.spGetAllProjects", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        //reading multiple Projects
                        while (sqlDataReader.Read())
                        {
                            Project project = new()
                            {
                                Id = sqlDataReader.GetInt32(0),
                                User_Id_Fk = sqlDataReader.GetInt32(1),
                                Name = sqlDataReader.GetString(2),
                                Description = sqlDataReader.IsDBNull(3) ? "--" : sqlDataReader.GetString(3),
                                Option = sqlDataReader.GetString(4),
                                Status = sqlDataReader.GetString(5),
                                Deadline = sqlDataReader.IsDBNull(6) ? "--" : sqlDataReader.GetDateTime(6).ToShortDateString(),
                                DateCreated = sqlDataReader.GetDateTime(7),
                                Logo = sqlDataReader.IsDBNull(8) ? null : ImageOperations.BytesToImage((byte[])sqlDataReader.GetValue(8))
                            };

                            projects.Add(project);
                        }
                    }
                    sqlDataReader.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return projects;
        }
        public IEnumerable<Project> FindAll(int userId, ProjectOptions projectOption)
        {
            List<Project> projects = new();

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Project.spFindAllProjects", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@project_option", SqlDbType.NVarChar, 50).Value = projectOption;

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        //reading multiple Projects
                        while (sqlDataReader.Read())
                        {
                            Project project = new()
                            {
                                Id = sqlDataReader.GetInt32(0),
                                User_Id_Fk = sqlDataReader.GetInt32(1),
                                Name = sqlDataReader.GetString(2),
                                Description = sqlDataReader.IsDBNull(3) ? "--" : sqlDataReader.GetString(3),
                                Option = sqlDataReader.GetString(4),
                                Status = sqlDataReader.GetString(5),
                                Deadline = sqlDataReader.IsDBNull(6) ? "--" : sqlDataReader.GetDateTime(6).ToShortDateString(),
                                DateCreated = sqlDataReader.GetDateTime(7),
                                Logo = sqlDataReader.IsDBNull(8) ? null : ImageOperations.BytesToImage((byte[])sqlDataReader.GetValue(8))
                            };

                            projects.Add(project);
                        }
                    }
                    sqlDataReader.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return projects;
        }
        public bool Update(Project project)
        {
            bool isUpdated = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Project.spUpdateProject", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = project.Id;
                    cmd.Parameters.Add("@project_name", SqlDbType.NVarChar, 50).Value = project.Name;
                    cmd.Parameters.Add("@project_description", SqlDbType.NVarChar, 500).Value = project.Description;
                    cmd.Parameters.Add("@project_option", SqlDbType.NVarChar, 50).Value = project.Option;
                    cmd.Parameters.Add("@project_status", SqlDbType.NVarChar, 12).Value = project.Status;
                    cmd.Parameters.Add("@project_deadline", SqlDbType.DateTime2).Value = project.Deadline;
                    cmd.Parameters.Add("@project_logo", SqlDbType.VarBinary).Value = project.Logo;

                    int NoOfRowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = NoOfRowsAffected > 0;
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

            return isUpdated;
        }
        public bool Delete(int userId)
        {
            bool isDeleted = false;

            if (userId != 0)
            {
                using SqlConnection con = new(_connectionString);
                using SqlCommand cmd = new("Project.spDeleteProject", con);
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = userId;

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
    }
}
