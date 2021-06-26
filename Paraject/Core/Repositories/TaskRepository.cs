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
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository()
        {
            _connectionString = ConnectionString.config;

        }
        public bool Add(Task task, int projectId)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Task.spAddTask", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = projectId;
                    cmd.Parameters.Add("@task_subject", SqlDbType.NVarChar, 50).Value = task.Subject;
                    cmd.Parameters.Add("@task_type", SqlDbType.NVarChar, 50).Value = task.Type;
                    cmd.Parameters.Add("@task_description", SqlDbType.NVarChar, 500).Value = task.Description;
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
                    if (ex.Number == 2627)// Violation of unique constraint (Subject should be unique)
                    {
                        MessageBox.Show($"{task.Subject} Already Exist !!!");
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
                                Description = sqlDataReader.GetString(taskDescription),
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
                    MessageBox.Show(ex.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return task;
        }
        public IEnumerable<Task> FindAll(int projectId, Statuses taskStatus, Priorities taskPriority, Categories taskCategory)
        {
            List<Task> tasks = null;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Task.spFindAllTasks", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = projectId;
                    cmd.Parameters.Add("@task_status", SqlDbType.NVarChar, 20).Value = taskStatus;
                    cmd.Parameters.Add("@task_priority", SqlDbType.NVarChar, 4).Value = taskPriority;
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
                        int taskType = sqlDataReader.GetOrdinal("task_type");
                        int taskDescription = sqlDataReader.GetOrdinal("task_description");
                        int taskStatusFromDb = sqlDataReader.GetOrdinal("task_status");
                        int taskCategoryFromDb = sqlDataReader.GetOrdinal("task_category");
                        int taskPriorityFromDb = sqlDataReader.GetOrdinal("task_priority");
                        int taskDeadline = sqlDataReader.GetOrdinal("task_deadline");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");

                        //reading multiple Tasks (Add a Task object to the tasks List if SqlDataReader returns a row from the Database)
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            Task task = new()
                            {
                                Id = sqlDataReader.GetInt32(taskIdFromDb),
                                Project_Id_Fk = sqlDataReader.GetInt32(projectIdFk),
                                Subject = sqlDataReader.GetString(taskSubject),
                                Type = sqlDataReader.GetString(taskType),
                                Description = sqlDataReader.GetString(taskDescription),
                                Status = sqlDataReader.GetString(taskStatusFromDb),
                                Category = sqlDataReader.GetString(taskCategoryFromDb),
                                Priority = sqlDataReader.GetString(taskPriorityFromDb),
                                Deadline = sqlDataReader.IsDBNull(taskDeadline) ? null : sqlDataReader.GetDateTime(taskDeadline),
                                DateCreated = sqlDataReader.GetDateTime(dateCreated)
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
                    MessageBox.Show(ex.ToString());
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
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
                    cmd.Parameters.Add("@task_subject", SqlDbType.NVarChar, 50).Value = task.Subject;
                    cmd.Parameters.Add("@task_type", SqlDbType.NVarChar, 50).Value = task.Type;
                    cmd.Parameters.Add("@task_description", SqlDbType.NVarChar, 500).Value = task.Description;
                    cmd.Parameters.Add("@task_status", SqlDbType.NVarChar, 20).Value = Enum.GetName(Statuses.Open);
                    cmd.Parameters.Add("@task_category", SqlDbType.NVarChar, 50).Value = task.Category;
                    cmd.Parameters.Add("@task_priority", SqlDbType.NVarChar, 4).Value = task.Priority;
                    cmd.Parameters.Add("@task_deadline", SqlDbType.DateTime2).Value = task.Deadline;


                    int rowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)// Violation of unique constraint (Subject should be unique)
                    {
                        MessageBox.Show($"{task.Subject} Already Exist !!!");
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
                    MessageBox.Show(ex.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return isDeleted;
        }
    }
}
