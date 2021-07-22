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
    public class TaskRepository : ITaskRepository
    {
        private readonly IDialogService _dialogService;
        private readonly string _connectionString;

        public TaskRepository()
        {
            _dialogService = new DialogService();
            _connectionString = ConnectionString.config;
        }

        public bool Add(Task task)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Task.spAddTask", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = task.Project_Id_Fk;
                    cmd.Parameters.Add("@task_subject", SqlDbType.NVarChar, 100).Value = task.Subject;
                    cmd.Parameters.Add("@task_type", SqlDbType.NVarChar, 50).Value = task.Type;
                    cmd.Parameters.Add("@task_description", SqlDbType.NVarChar, 1515).Value = task.Description;
                    cmd.Parameters.Add("@task_status", SqlDbType.NVarChar, 20).Value = Enum.GetName(Statuses.Open);
                    cmd.Parameters.Add("@task_category", SqlDbType.NVarChar, 50).Value = task.Category;
                    cmd.Parameters.Add("@task_priority", SqlDbType.NVarChar, 4).Value = task.Priority;
                    cmd.Parameters.Add("@task_deadline", SqlDbType.DateTime2).Value = task.Deadline;
                    cmd.Parameters.Add("@date_created", SqlDbType.DateTime2).Value = DateTime.Now;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isAdded = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
            }

            return isAdded;
        }
        public Task Get(int taskId)
        {
            Task task = null;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Task.spGetTask", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@task_id", SqlDbType.Int).Value = taskId;

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        //Move to the first record.  If no records, get out.
                        if (!sqlDataReader.Read()) { return null; }

                        //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                        int taskIdFromDb = sqlDataReader.GetOrdinal("task_id");
                        int projectIdFk = sqlDataReader.GetOrdinal("project_id");
                        int taskSubject = sqlDataReader.GetOrdinal("task_subject");
                        int taskType = sqlDataReader.GetOrdinal("task_type");
                        int taskDescription = sqlDataReader.GetOrdinal("task_description");
                        int taskStatus = sqlDataReader.GetOrdinal("task_status");
                        int taskCategory = sqlDataReader.GetOrdinal("task_category");
                        int taskPriority = sqlDataReader.GetOrdinal("task_priority");
                        int taskDeadline = sqlDataReader.GetOrdinal("task_deadline");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");

                        //Reads a single Task
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            task = new Task()
                            {
                                Id = sqlDataReader.GetInt32(taskIdFromDb),
                                Project_Id_Fk = sqlDataReader.GetInt32(projectIdFk),
                                Subject = sqlDataReader.GetString(taskSubject),
                                Type = sqlDataReader.GetString(taskType),
                                Description = sqlDataReader.IsDBNull(taskDescription) ? "--" : sqlDataReader.GetString(taskDescription),
                                Status = sqlDataReader.GetString(taskStatus),
                                Category = sqlDataReader.GetString(taskCategory),
                                Priority = sqlDataReader.GetString(taskPriority),
                                Deadline = sqlDataReader.IsDBNull(taskDeadline) ? null : sqlDataReader.GetDateTime(taskDeadline),
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
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
            }

            return task;
        }
        public IEnumerable<Task> FindAll(int projectId, string taskType, string taskStatus, string taskPriority, string taskCategory)
        {
            List<Task> tasks = new();

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Task.spFindAllTasks", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = projectId;
                    cmd.Parameters.Add("@task_type", SqlDbType.NVarChar, 50).Value = taskType;
                    cmd.Parameters.Add("@task_status", SqlDbType.NVarChar, 20).Value = taskStatus;
                    cmd.Parameters.Add("@task_priority", SqlDbType.NVarChar, 8).Value = taskPriority;
                    cmd.Parameters.Add("@task_category", SqlDbType.NVarChar, 50).Value = taskCategory;

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        // Move to the first record.  If no records, get out.
                        if (!sqlDataReader.Read()) return null;

                        //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                        int taskIdFromDb = sqlDataReader.GetOrdinal("task_id");
                        int projectIdFk = sqlDataReader.GetOrdinal("project_id");
                        int taskSubject = sqlDataReader.GetOrdinal("task_subject");
                        int taskTypeFromDb = sqlDataReader.GetOrdinal("task_type");
                        int taskDescription = sqlDataReader.GetOrdinal("task_description");
                        int taskStatusFromDb = sqlDataReader.GetOrdinal("task_status");
                        int taskCategoryFromDb = sqlDataReader.GetOrdinal("task_category");
                        int taskPriorityFromDb = sqlDataReader.GetOrdinal("task_priority");
                        int taskDeadline = sqlDataReader.GetOrdinal("task_deadline");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");
                        int subtaskCount = sqlDataReader.GetOrdinal("subtask_count"); // subtask_count is not a column in Subtask table, this is just an alias found in spFindAllTasks

                        //reading multiple Tasks (Add a Task object to the tasks List if SqlDataReader returns a row from the Database)
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            Task task = new()
                            {
                                Id = sqlDataReader.GetInt32(taskIdFromDb),
                                Project_Id_Fk = sqlDataReader.GetInt32(projectIdFk),
                                Subject = sqlDataReader.GetString(taskSubject),
                                Type = sqlDataReader.GetString(taskTypeFromDb),
                                Description = sqlDataReader.IsDBNull(taskDescription) ? "--" : sqlDataReader.GetString(taskDescription),
                                Status = sqlDataReader.GetString(taskStatusFromDb),
                                Category = sqlDataReader.GetString(taskCategoryFromDb).Replace("_", " "),
                                Priority = sqlDataReader.GetString(taskPriorityFromDb),
                                Deadline = sqlDataReader.IsDBNull(taskDeadline) ? null : sqlDataReader.GetDateTime(taskDeadline),
                                DateCreated = sqlDataReader.GetDateTime(dateCreated),
                                SubtaskCount = sqlDataReader.IsDBNull(subtaskCount) ? 0 : sqlDataReader.GetInt32(subtaskCount)
                            };

                            tasks.Add(task);
                        }
                        //reading multiple Projects
                        while (sqlDataReader.Read());
                    }
                    sqlDataReader.Close();
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
            }

            return tasks;
        }
        public bool Update(Task task)
        {
            bool isUpdated = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Task.spUpdateTask", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@task_id", SqlDbType.Int).Value = task.Id;
                    cmd.Parameters.Add("@task_subject", SqlDbType.NVarChar, 100).Value = task.Subject;
                    cmd.Parameters.Add("@task_type", SqlDbType.NVarChar, 50).Value = task.Type;
                    cmd.Parameters.Add("@task_description", SqlDbType.NVarChar, 1515).Value = task.Description;
                    cmd.Parameters.Add("@task_status", SqlDbType.NVarChar, 20).Value = task.Status;
                    cmd.Parameters.Add("@task_category", SqlDbType.NVarChar, 50).Value = task.Category;
                    cmd.Parameters.Add("@task_priority", SqlDbType.NVarChar, 4).Value = task.Priority;
                    cmd.Parameters.Add("@task_deadline", SqlDbType.DateTime2).Value = task.Deadline;


                    int rowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
            }

            return isUpdated;
        }
        public bool Delete(int taskId)
        {
            bool isDeleted = false;

            if (taskId <= 0) { return false; }

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Task.spDeleteTask", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@task_id", SqlDbType.Int).Value = taskId;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isDeleted = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidTask));
                }
            }

            return isDeleted;
        }
    }
}
