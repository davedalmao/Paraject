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
    public class NoteRepository : INoteRepository
    {
        private readonly string _connectionString;

        public NoteRepository()
        {
            _connectionString = ConnectionString.config;

        }

        public bool Add(Note note, int projectId)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Note.spAddNote", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = projectId;
                    cmd.Parameters.Add("@note_subject", SqlDbType.NVarChar, 50).Value = note.Subject;
                    cmd.Parameters.Add("@note_description", SqlDbType.NVarChar, 1515).Value = string.IsNullOrWhiteSpace(note.Description) ? null : note.Description;
                    cmd.Parameters.Add("@date_created", SqlDbType.DateTime2).Value = DateTime.Now;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isAdded = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"An SQL error occured while processing data. \nError: { ex.Message }");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return isAdded;
        }
        public Note Get(int noteId)
        {
            Note note = null;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Note.spGetNote", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@note_id", SqlDbType.Int).Value = noteId;

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        //Move to the first record.  If no records, get out.
                        if (!sqlDataReader.Read()) { return null; }

                        //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                        int noteIdFromDb = sqlDataReader.GetOrdinal("note_id");
                        int projectIdFk = sqlDataReader.GetOrdinal("project_id");
                        int noteSubject = sqlDataReader.GetOrdinal("note_subject");
                        int noteDescription = sqlDataReader.GetOrdinal("note_description");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");

                        //Reads a single Note
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            note = new Note()
                            {
                                Id = sqlDataReader.GetInt32(noteIdFromDb),
                                Project_Id_Fk = sqlDataReader.GetInt32(projectIdFk),
                                Subject = sqlDataReader.GetString(noteSubject),
                                Description = sqlDataReader.GetString(noteDescription),
                                DateCreated = sqlDataReader.GetDateTime(dateCreated)
                            };
                        }
                        while (sqlDataReader.Read());
                    }
                    sqlDataReader.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"An SQL error occured while processing data. \nError: { ex.Message }");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return note;
        }
        public IEnumerable<Note> GetAll(int projectId)
        {
            throw new NotImplementedException();
        }
        public bool Update(Note note)
        {
            throw new NotImplementedException();
        }
        public bool Delete(int noteId)
        {
            throw new NotImplementedException();
        }
    }
}
