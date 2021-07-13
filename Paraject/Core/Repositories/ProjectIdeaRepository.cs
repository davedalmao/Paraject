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
    public class ProjectIdeaRepository : IProjectIdeaRepository
    {
        private readonly string _connectionString;

        public ProjectIdeaRepository()
        {
            _connectionString = ConnectionString.config;

        }

        public bool Add(ProjectIdea projectIdea, int userId)
        {
            bool isAdded = false;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Project_Idea.spAddProjectIdea", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@project_idea_name", SqlDbType.NVarChar, 50).Value = projectIdea.Name;
                    cmd.Parameters.Add("@project_idea_description", SqlDbType.NVarChar, 1515).Value = string.IsNullOrWhiteSpace(projectIdea.Description) ? null : projectIdea.Description;
                    cmd.Parameters.Add("@project_idea_features", SqlDbType.NVarChar, 1515).Value = projectIdea.Features;
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
        public ProjectIdea Get(int projectIdeaId)
        {
            ProjectIdea projectIdea = null;

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Project_Idea.spGetProjectIdea", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@project_idea_id", SqlDbType.Int).Value = projectIdeaId;

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        //Move to the first record.  If no records, get out.
                        if (!sqlDataReader.Read()) { return null; }

                        //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                        int projectIdeaIdFromDb = sqlDataReader.GetOrdinal("project_idea_id");
                        int userIdFk = sqlDataReader.GetOrdinal("user_id");
                        int projectIdeaName = sqlDataReader.GetOrdinal("project_idea_name");
                        int projectIdeaDescription = sqlDataReader.GetOrdinal("project_idea_description");
                        int projectIdeaFeatures = sqlDataReader.GetOrdinal("project_idea_features");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");

                        //Reads a single ProjectIdea
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            projectIdea = new ProjectIdea()
                            {
                                Id = sqlDataReader.GetInt32(projectIdeaIdFromDb),
                                User_Id_Fk = sqlDataReader.GetInt32(userIdFk),
                                Name = sqlDataReader.GetString(projectIdeaName),
                                Description = sqlDataReader.GetString(projectIdeaDescription),
                                Features = sqlDataReader.GetString(projectIdeaFeatures),
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

            return projectIdea;
        }
        public IEnumerable<ProjectIdea> GetAll(int userId)
        {
            List<ProjectIdea> projectIdeas = new();

            using (SqlConnection con = new(_connectionString))
            using (SqlCommand cmd = new("Project_Idea.spGetAllProjectIdeas", con))
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
                        if (!sqlDataReader.Read()) { return null; }

                        //Ordinals (Gets the column number from the database based on the [column name] passed in GetOrdinal method)
                        int projectIdeaIdFromDb = sqlDataReader.GetOrdinal("project_idea_id");
                        int userIdFk = sqlDataReader.GetOrdinal("user_id");
                        int projectIdeaName = sqlDataReader.GetOrdinal("project_idea_name");
                        int projectIdeaDescription = sqlDataReader.GetOrdinal("project_idea_description");
                        int projectIdeaFeatures = sqlDataReader.GetOrdinal("project_idea_features");
                        int dateCreated = sqlDataReader.GetOrdinal("date_created");

                        //reading multiple ProjectIdeas (Add a ProjectIdea object to the projectIdeas List if SqlDataReader returns a row from the Database)
                        //Remember, we're already on the first record, so use do/while here.
                        do
                        {
                            ProjectIdea projectIdea = new()
                            {
                                Id = sqlDataReader.GetInt32(projectIdeaIdFromDb),
                                User_Id_Fk = sqlDataReader.GetInt32(userIdFk),
                                Name = sqlDataReader.GetString(projectIdeaName),
                                Description = sqlDataReader.IsDBNull(projectIdeaDescription) ? "--" : sqlDataReader.GetString(projectIdeaDescription),
                                Features = sqlDataReader.GetString(projectIdeaFeatures),
                                DateCreated = sqlDataReader.GetDateTime(dateCreated)
                            };

                            projectIdeas.Add(projectIdea);
                        }
                        //reading multiple ProjectIdeas
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

            return projectIdeas;
        }
        public bool Update(ProjectIdea projectIdea)
        {
            throw new NotImplementedException();
        }
        public bool Delete(int projectIdeaId)
        {
            throw new NotImplementedException();
        }
    }
}
