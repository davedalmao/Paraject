using Paraject.Core.Converters;
using Paraject.Core.Enums;
using Paraject.Core.Repositories.Interfaces;
using Paraject.Core.Services.DialogService;
using Paraject.Core.Utilities;
using Paraject.MVVM.Models;
using Paraject.MVVM.ViewModels.MessageBoxes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Paraject.Core.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDialogService _dialogService;
        private readonly string _connectionString;

        public ProjectRepository()
        {
            _dialogService = new DialogService();
            _connectionString = ConnectionString.config;
        }

        public bool Add(Project project)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Project.spAddProject", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = project.User_Id_Fk;
                    cmd.Parameters.Add("@project_name", SqlDbType.NVarChar, 100).Value = project.Name;
                    cmd.Parameters.Add("@project_description", SqlDbType.NVarChar, 1515).Value = string.IsNullOrWhiteSpace(project.Description) ? null : project.Description;
                    cmd.Parameters.Add("@project_option", SqlDbType.NVarChar, 50).Value = project.Option;
                    cmd.Parameters.Add("@project_status", SqlDbType.NVarChar, 12).Value = Enum.GetName(Statuses.Open);
                    cmd.Parameters.Add("@project_deadline", SqlDbType.DateTime2).Value = project.Deadline;
                    cmd.Parameters.Add("@project_logo", SqlDbType.VarBinary).Value = ImageConverter.ImageToBytes(project.Logo);
                    cmd.Parameters.Add("@date_created", SqlDbType.DateTime2).Value = DateTime.Now;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isAdded = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
                }
            }
            return isAdded;
        }
        public Project Get(int projectId)
        {
            Project project = null;

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("Project.spGetProject", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = projectId;

                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    //Move to the first record.  If no records, get out.
                    if (!sqlDataReader.Read()) { return null; }

                    //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                    int projectIdFromDb = sqlDataReader.GetOrdinal("project_id");
                    int userIdFk = sqlDataReader.GetOrdinal("user_id");
                    int projectName = sqlDataReader.GetOrdinal("project_name");
                    int projectDescription = sqlDataReader.GetOrdinal("project_description");
                    int projectOption = sqlDataReader.GetOrdinal("project_option");
                    int projectStatus = sqlDataReader.GetOrdinal("project_status");
                    int projectDeadline = sqlDataReader.GetOrdinal("project_deadline");
                    int dateCreated = sqlDataReader.GetOrdinal("date_created");
                    int projectLogo = sqlDataReader.GetOrdinal("project_logo");
                    int taskCount = sqlDataReader.GetOrdinal("task_count");

                    //Reads a single Project
                    //Remember, we're already on the first record, so use do/while here.
                    do
                    {
                        project = new Project()
                        {
                            Id = sqlDataReader.GetInt32(projectIdFromDb),
                            User_Id_Fk = sqlDataReader.GetInt32(userIdFk),
                            Name = sqlDataReader.GetString(projectName),
                            Description = sqlDataReader.IsDBNull(projectDescription) ? "--" : sqlDataReader.GetString(projectDescription),
                            Option = sqlDataReader.GetString(projectOption),
                            Status = sqlDataReader.GetString(projectStatus),
                            Deadline = sqlDataReader.IsDBNull(projectDeadline) ? null : sqlDataReader.GetDateTime(projectDeadline),
                            DateCreated = sqlDataReader.GetDateTime(dateCreated),
                            Logo = sqlDataReader.IsDBNull(projectLogo) ? null : ImageConverter.BytesToImage((byte[])sqlDataReader.GetValue(projectLogo)),
                            TaskCount = sqlDataReader.IsDBNull(taskCount) ? 0 : sqlDataReader.GetInt32(taskCount) //task_count is not a column in Project table, this is just an alias found in Project.spGetProject
                        };
                    }
                    while (sqlDataReader.Read());
                }
                sqlDataReader.Close();
            }
            catch (SqlException ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                         $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
            }
            catch (Exception ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
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
                        //Move to the first record.  If no records, get out.
                        if (!sqlDataReader.Read()) return null;

                        //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                        int projectId = sqlDataReader.GetOrdinal("project_id");
                        int userIdFk = sqlDataReader.GetOrdinal("user_id");
                        int projectName = sqlDataReader.GetOrdinal("project_name");
                        int projectDescription = sqlDataReader.GetOrdinal("project_description");
                        int projectOption = sqlDataReader.GetOrdinal("project_option");
                        int projectStatus = sqlDataReader.GetOrdinal("project_status");
                        int projectDeadline = sqlDataReader.GetOrdinal("project_deadline");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");
                        int projectLogo = sqlDataReader.GetOrdinal("project_logo");

                        //reading multiple Projects (Add a Project object to the projects List if SqlDataReader returns a row from the Database)
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            Project project = new()
                            {
                                Id = sqlDataReader.GetInt32(projectId),
                                User_Id_Fk = sqlDataReader.GetInt32(userIdFk),
                                Name = sqlDataReader.GetString(projectName),
                                Description = sqlDataReader.IsDBNull(projectDescription) ? "--" : sqlDataReader.GetString(projectDescription),
                                Option = sqlDataReader.GetString(projectOption),
                                Status = sqlDataReader.GetString(projectStatus).Replace("_", " "),
                                Deadline = sqlDataReader.IsDBNull(projectDeadline) ? null : sqlDataReader.GetDateTime(projectDeadline),
                                DateCreated = sqlDataReader.GetDateTime(dateCreated),
                                Logo = sqlDataReader.IsDBNull(projectLogo) ? null : ImageConverter.BytesToImage((byte[])sqlDataReader.GetValue(projectLogo))
                            };

                            projects.Add(project);
                        }
                        //reading multiple Projects
                        while (sqlDataReader.Read());
                    }
                    sqlDataReader.Close();
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
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
                        // Move to the first record.  If no records, get out.
                        if (!sqlDataReader.Read()) return null;

                        //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                        int projectId = sqlDataReader.GetOrdinal("project_id");
                        int userIdFk = sqlDataReader.GetOrdinal("user_id");
                        int projectName = sqlDataReader.GetOrdinal("project_name");
                        int projectDescription = sqlDataReader.GetOrdinal("project_description");
                        int projectOptionFromDb = sqlDataReader.GetOrdinal("project_option");
                        int projectStatus = sqlDataReader.GetOrdinal("project_status");
                        int projectDeadline = sqlDataReader.GetOrdinal("project_deadline");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");
                        int projectLogo = sqlDataReader.GetOrdinal("project_logo");

                        //reading multiple Projects (Add a Project object to the projects List if SqlDataReader returns a row from the Database)
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            Project project = new()
                            {
                                Id = sqlDataReader.GetInt32(projectId),
                                User_Id_Fk = sqlDataReader.GetInt32(userIdFk),
                                Name = sqlDataReader.GetString(projectName),
                                Description = sqlDataReader.IsDBNull(projectDescription) ? "--" : sqlDataReader.GetString(projectDescription),
                                Option = sqlDataReader.GetString(projectOptionFromDb),
                                Status = sqlDataReader.GetString(projectStatus).Replace("_", " "),
                                Deadline = sqlDataReader.IsDBNull(projectDeadline) ? null : sqlDataReader.GetDateTime(projectDeadline),
                                DateCreated = sqlDataReader.GetDateTime(dateCreated),
                                Logo = sqlDataReader.IsDBNull(projectLogo) ? null : ImageConverter.BytesToImage((byte[])sqlDataReader.GetValue(projectLogo))
                            };

                            projects.Add(project);
                        }
                        //reading multiple Projects
                        while (sqlDataReader.Read());
                    }
                    sqlDataReader.Close();
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
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
                    cmd.Parameters.Add("@project_name", SqlDbType.NVarChar, 100).Value = project.Name;
                    cmd.Parameters.Add("@project_description", SqlDbType.NVarChar, 1515).Value = string.IsNullOrWhiteSpace(project.Description) ? null : project.Description;
                    cmd.Parameters.Add("@project_option", SqlDbType.NVarChar, 50).Value = project.Option;
                    cmd.Parameters.Add("@project_status", SqlDbType.NVarChar, 12).Value = project.Status;
                    cmd.Parameters.Add("@project_deadline", SqlDbType.DateTime2).Value = project.Deadline;
                    cmd.Parameters.Add("@project_logo", SqlDbType.VarBinary).Value = ImageConverter.ImageToBytes(project.Logo);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
                }
            }

            return isUpdated;
        }
        public bool Delete(int projectId)
        {
            bool isDeleted = false;

            if (projectId <= 0) { return false; }

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("Project.spDeleteProject", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = projectId;

                int rowsAffected = cmd.ExecuteNonQuery();
                isDeleted = rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                         $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
            }
            catch (Exception ex)
            {
                _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidProject));
            }

            return isDeleted;
        }
    }
}
