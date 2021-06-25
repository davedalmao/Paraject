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
                    cmd.Parameters.Add("@task_status", SqlDbType.NVarChar, 20).Value = Enum.GetName(Status.Open);
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

        public bool Delete(int taskId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> FindAll(int projectId, Status taskStatus, Priority taskPriority, Category taskCategory)
        {
            throw new NotImplementedException();
        }

        public Task Get(int taskId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Task task)
        {
            throw new NotImplementedException();
        }
    }
}
