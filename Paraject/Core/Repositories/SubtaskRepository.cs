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
    public class SubtaskRepository : ISubtaskRepository
    {
        private readonly string _connectionString;

        public SubtaskRepository()
        {
            _connectionString = ConnectionString.config;

        }

        public bool Add(Subtask subtask, int taskId)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Subtask.spAddSubtask", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@task_id", SqlDbType.Int).Value = taskId;
                    cmd.Parameters.Add("@subtask_subject", SqlDbType.NVarChar, 50).Value = subtask.Subject;
                    cmd.Parameters.Add("@subtask_status", SqlDbType.NVarChar, 20).Value = Enum.GetName(Statuses.Open);
                    cmd.Parameters.Add("@subtask_priority", SqlDbType.NVarChar, 4).Value = subtask.Priority;
                    cmd.Parameters.Add("@subtask_description", SqlDbType.NVarChar, 1515).Value = string.IsNullOrWhiteSpace(subtask.Description) ? null : subtask.Description;
                    cmd.Parameters.Add("@subtask_deadline", SqlDbType.DateTime2).Value = subtask.Deadline;
                    cmd.Parameters.Add("@date_created", SqlDbType.DateTime2).Value = DateTime.Now;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isAdded = rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return isAdded;
        }
        public Subtask Get(int subtaskId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Subtask> GetAll(int taskId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Subtask> FindAll(int taskId, string subtaskStatus, string subtaskPriority)
        {
            throw new NotImplementedException();
        }
        public bool Update(Subtask subtask)
        {
            throw new NotImplementedException();
        }
        public bool Delete(int subtaskId)
        {
            throw new NotImplementedException();
        }
    }
}
