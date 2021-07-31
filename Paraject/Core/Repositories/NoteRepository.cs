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
    public class NoteRepository : INoteRepository
    {
        private readonly IDialogService _dialogService;
        private readonly string _connectionString;

        public NoteRepository()
        {
            _dialogService = new DialogService();
            _connectionString = ConnectionString.config;
        }

        public bool Add(Note note)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Note.spAddNote", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = note.Project_Id_Fk;
                    cmd.Parameters.Add("@note_subject", SqlDbType.NVarChar, 50).Value = note.Subject;
                    cmd.Parameters.Add("@note_description", SqlDbType.NVarChar, 1515).Value = string.IsNullOrWhiteSpace(note.Description) ? null : note.Description;
                    cmd.Parameters.Add("@date_created", SqlDbType.DateTime2).Value = DateTime.Now;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isAdded = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
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
                                Description = sqlDataReader.IsDBNull(noteDescription) ? "--" : sqlDataReader.GetString(noteDescription),
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
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
                }
            }

            return note;
        }
        public IEnumerable<Note> GetAll(int projectId)
        {
            List<Note> notes = new();

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Note.spGetAllNotes", con))
            {
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
                        int noteIdFromDb = sqlDataReader.GetOrdinal("note_id");
                        int projectIdFk = sqlDataReader.GetOrdinal("project_id");
                        int noteSubject = sqlDataReader.GetOrdinal("note_subject");
                        int noteDescription = sqlDataReader.GetOrdinal("note_description");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");

                        //reading multiple Notes (Add a Note object to the notes List if SqlDataReader returns a row from the Database)
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            Note note = new()
                            {
                                Id = sqlDataReader.GetInt32(noteIdFromDb),
                                Project_Id_Fk = sqlDataReader.GetInt32(projectIdFk),
                                Subject = sqlDataReader.GetString(noteSubject),
                                Description = sqlDataReader.IsDBNull(noteDescription) ? null : sqlDataReader.GetString(noteDescription),
                                DateCreated = sqlDataReader.GetDateTime(dateCreated)
                            };

                            notes.Add(note);
                        }
                        //reading multiple Notes
                        while (sqlDataReader.Read());
                    }
                    sqlDataReader.Close();
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
                }
            }

            return notes;
        }
        public bool Update(Note note)
        {
            bool isUpdated = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Note.spUpdateNote", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@note_id", SqlDbType.Int).Value = note.Id;
                    cmd.Parameters.Add("@note_subject", SqlDbType.NVarChar, 50).Value = note.Subject;
                    cmd.Parameters.Add("@note_description", SqlDbType.NVarChar, 1515).Value = string.IsNullOrWhiteSpace(note.Description) ? null : note.Description;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
                }
            }

            return isUpdated;
        }
        public bool Delete(int noteId)
        {
            bool isDeleted = false;

            if (noteId <= 0) { return false; }

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Note.spDeleteNote", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@note_id", SqlDbType.Int).Value = noteId;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isDeleted = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error",
                                             $"An SQL error occured while processing data: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
                }
                catch (Exception ex)
                {
                    _dialogService.OpenDialog(new OkayMessageBoxViewModel("Error", $"An error occured: \n\n{ ex.Message } \n\n{ ex.StackTrace }", Icon.InvalidNote));
                }
            }

            return isDeleted;
        }
    }
}
