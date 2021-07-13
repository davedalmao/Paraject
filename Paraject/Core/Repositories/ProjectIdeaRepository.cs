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
            throw new NotImplementedException();
        }
        public IEnumerable<ProjectIdea> GetAll(int userId)
        {
            throw new NotImplementedException();
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
