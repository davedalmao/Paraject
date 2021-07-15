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

        public bool Add(Subtask subtask)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Subtask.spAddSubtask", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@task_id", SqlDbType.Int).Value = subtask.Task_Id_Fk;
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
            Subtask subtask = null;

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("Subtask.spGetSubtask", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@subtask_id", SqlDbType.Int).Value = subtaskId;

                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    //Move to the first record.  If no records, get out.
                    if (!sqlDataReader.Read()) { return null; }

                    //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                    int subtaskIdFromDb = sqlDataReader.GetOrdinal("subtask_id");
                    int taskIdFk = sqlDataReader.GetOrdinal("task_id");
                    int subtaskSubject = sqlDataReader.GetOrdinal("subtask_subject");
                    int subtaskStatus = sqlDataReader.GetOrdinal("subtask_status");
                    int subtaskPriority = sqlDataReader.GetOrdinal("subtask_priority");
                    int subtaskDescription = sqlDataReader.GetOrdinal("subtask_description");
                    int subtaskDeadline = sqlDataReader.GetOrdinal("subtask_deadline");
                    int dateCreated = sqlDataReader.GetOrdinal("date_created");

                    //Reads a single Project
                    //Remember, we're already on the first record, so use do/while here.
                    do
                    {
                        subtask = new Subtask()
                        {
                            Id = sqlDataReader.GetInt32(subtaskIdFromDb),
                            Task_Id_Fk = sqlDataReader.GetInt32(taskIdFk),
                            Subject = sqlDataReader.GetString(subtaskSubject),
                            Status = sqlDataReader.GetString(subtaskStatus),
                            Priority = sqlDataReader.GetString(subtaskPriority),
                            Description = sqlDataReader.IsDBNull(subtaskDescription) ? "--" : sqlDataReader.GetString(subtaskDescription),
                            Deadline = sqlDataReader.IsDBNull(subtaskDeadline) ? null : sqlDataReader.GetDateTime(subtaskDeadline),
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

            return subtask;
        }
        public IEnumerable<Subtask> GetAll(int taskId)
        {
            List<Subtask> subtasks = new();

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Subtask.spGetAllSubtasks", con))
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
                        int subtaskIdFromDb = sqlDataReader.GetOrdinal("subtask_id");
                        int taskIdFk = sqlDataReader.GetOrdinal("task_id");
                        int subtaskSubject = sqlDataReader.GetOrdinal("subtask_subject");
                        int subtaskStatus = sqlDataReader.GetOrdinal("subtask_status");
                        int subtaskPriority = sqlDataReader.GetOrdinal("subtask_priority");
                        int subtaskDescription = sqlDataReader.GetOrdinal("subtask_description");
                        int subtaskDeadline = sqlDataReader.GetOrdinal("subtask_deadline");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");

                        //reading multiple Subtasks (Add a Subtask object to the subtasks List if SqlDataReader returns a row from the Database)
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            Subtask subtask = new()
                            {
                                Id = sqlDataReader.GetInt32(subtaskIdFromDb),
                                Task_Id_Fk = sqlDataReader.GetInt32(taskIdFk),
                                Subject = sqlDataReader.GetString(subtaskSubject),
                                Status = sqlDataReader.GetString(subtaskStatus),

                                //when I query for Completed(status) subtasks, Priority property has no value, therefore not showing the Priority in the UI (Subtask Card)
                                Priority = (sqlDataReader.GetString(subtaskStatus) == "Completed") ? null : sqlDataReader.GetString(subtaskPriority),

                                Description = sqlDataReader.IsDBNull(subtaskDescription) ? "--" : sqlDataReader.GetString(subtaskDescription),
                                Deadline = sqlDataReader.IsDBNull(subtaskDeadline) ? null : sqlDataReader.GetDateTime(subtaskDeadline),
                                DateCreated = sqlDataReader.GetDateTime(dateCreated)
                            };

                            subtasks.Add(subtask);
                        }
                        //reading multiple Subtasks
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

            return subtasks;
        }
        public IEnumerable<Subtask> FindAll(int taskId, string subtaskStatus, string subtaskPriority)
        {
            List<Subtask> subtasks = new();

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Subtask.spFindAllSubtasks", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@task_id", SqlDbType.Int).Value = taskId;
                    cmd.Parameters.Add("@subtask_status", SqlDbType.NVarChar, 20).Value = subtaskStatus;
                    cmd.Parameters.Add("@subtask_priority", SqlDbType.NVarChar, 8).Value = subtaskPriority;

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        //Move to the first record.  If no records, get out.
                        if (!sqlDataReader.Read()) { return null; }

                        //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                        int subtaskIdFromDb = sqlDataReader.GetOrdinal("subtask_id");
                        int taskIdFk = sqlDataReader.GetOrdinal("task_id");
                        int subtaskSubject = sqlDataReader.GetOrdinal("subtask_subject");
                        int subtaskStatusFromDb = sqlDataReader.GetOrdinal("subtask_status");
                        int subtaskPriorityFromDb = sqlDataReader.GetOrdinal("subtask_priority");
                        int subtaskDescription = sqlDataReader.GetOrdinal("subtask_description");
                        int subtaskDeadline = sqlDataReader.GetOrdinal("subtask_deadline");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");

                        //reading multiple Subtasks (Add a Subtask object to the subtasks List if SqlDataReader returns a row from the Database)
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            Subtask subtask = new()
                            {
                                Id = sqlDataReader.GetInt32(subtaskIdFromDb),
                                Task_Id_Fk = sqlDataReader.GetInt32(taskIdFk),
                                Subject = sqlDataReader.GetString(subtaskSubject),
                                Status = sqlDataReader.GetString(subtaskStatusFromDb),
                                Priority = sqlDataReader.GetString(subtaskPriorityFromDb),
                                Description = sqlDataReader.IsDBNull(subtaskDescription) ? "--" : sqlDataReader.GetString(subtaskDescription),
                                Deadline = sqlDataReader.IsDBNull(subtaskDeadline) ? null : sqlDataReader.GetDateTime(subtaskDeadline),
                                DateCreated = sqlDataReader.GetDateTime(dateCreated)
                            };

                            subtasks.Add(subtask);
                        }
                        //reading multiple Subtasks
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

            return subtasks;
        }
        public bool Update(Subtask subtask)
        {
            bool isUpdated = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Subtask.spUpdateSubtask", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@subtask_id", SqlDbType.Int).Value = subtask.Id;
                    cmd.Parameters.Add("@subtask_subject", SqlDbType.NVarChar, 50).Value = subtask.Subject;
                    cmd.Parameters.Add("@subtask_status", SqlDbType.NVarChar, 20).Value = subtask.Status;
                    cmd.Parameters.Add("@subtask_priority", SqlDbType.NVarChar, 4).Value = subtask.Priority;
                    cmd.Parameters.Add("@subtask_description", SqlDbType.NVarChar, 1515).Value = string.IsNullOrWhiteSpace(subtask.Description) ? null : subtask.Description;
                    cmd.Parameters.Add("@subtask_deadline", SqlDbType.DateTime2).Value = subtask.Deadline;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return isUpdated;
        }
        public bool Delete(int subtaskId)
        {
            bool isDeleted = false;

            if (subtaskId <= 0) { return false; }

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("Subtask.spDeleteSubtask", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@subtask_id", SqlDbType.Int).Value = subtaskId;

                int rowsAffected = cmd.ExecuteNonQuery();
                isDeleted = rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return isDeleted;
        }
    }
}
