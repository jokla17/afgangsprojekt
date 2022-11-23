﻿using System.Data;
using System.Data.SqlClient;

namespace API.Repositories
{
    public class CompumatRepository
    {
        SqlConnection sqlConnection = new SqlConnection(API.ConnectionString.GetConnectionString());

        //read one from table
        public async Task<Compumat> GetCompumat(int id)
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM db.dbo.Compumat WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            if (!sqlDataReader.HasRows) {
                sqlConnection.Close();
                return null;
            }
            Compumat result = new Compumat();
            while (sqlDataReader.Read())
            {
                result = ParseCompumat(sqlDataReader);
            }
            sqlConnection.Close();
            return result;
        }

        //read all from table
        public async Task<List<Compumat>> GetAllCompumats()
        {
            List<Compumat> compumats = new List<Compumat>();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM db.dbo.Compumat", sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            if (!sqlDataReader.HasRows) {
                sqlConnection.Close();
                return null;
            }
            while (sqlDataReader.Read())
            {
                Compumat compumat = new Compumat();
                compumat = ParseCompumat(sqlDataReader);
                compumats.Add(compumat);
            }
            sqlConnection.Close();
            return compumats;
        }


        //create one in table
        public async Task<Compumat> CreateCompumat(Compumat compumat)
        {
            SqlDataAdapter sql = new SqlDataAdapter();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO db.dbo.Compumat (Name, Latitude, Longitude, Type, Status) OUTPUT INSERTED.* VALUES (@Name, @Longitude, @Latitude, @Type, @Status)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Name", compumat.Name);
            sqlCommand.Parameters.AddWithValue("@Longitude", compumat.Longitude);
            sqlCommand.Parameters.AddWithValue("@Latitude", compumat.Latitude);
            sqlCommand.Parameters.AddWithValue("@Type", (int)compumat.Type);
            sqlCommand.Parameters.AddWithValue("@Status", compumat.Status);
            sql.UpdateCommand = sqlCommand;
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            if (!sqlDataReader.HasRows) {
                sqlConnection.Close();
                return null;
            }
            Compumat result = new Compumat();
            while (sqlDataReader.Read())
            {
                result = ParseCompumat(sqlDataReader);
            }
            sqlConnection.Close();
            return result;
        }

        //update one in table
        public async Task<Compumat> UpdateCompumat(Compumat compumat)
        {
            SqlDataAdapter sql = new SqlDataAdapter();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("UPDATE db.dbo.Compumat SET Name = @Name, Latitude = @Latitude, Longitude = @Longitude, Type = @Type, Status = @Status WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", compumat.Id);
            sqlCommand.Parameters.AddWithValue("@Name", compumat.Name);
            sqlCommand.Parameters.AddWithValue("@Latitude", compumat.Latitude);
            sqlCommand.Parameters.AddWithValue("@Longitude", compumat.Longitude);
            sqlCommand.Parameters.AddWithValue("@Type", (int)compumat.Type);
            sqlCommand.Parameters.AddWithValue("@Status", compumat.Status);
            sql.UpdateCommand = sqlCommand;
            var resp = await sqlCommand.ExecuteNonQueryAsync();

            if (resp < 1) {
                sqlConnection.Close();
                return null;
            }

            Compumat result = new Compumat();
            if(resp > 0)
            {
               sqlConnection.Close();
               result = await GetCompumat((int)compumat.Id);
            }

            return result;
        }

        //delete one in table
        public async Task<string> DeleteCompumat(string id)
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("DELETE FROM db.dbo.Compumat WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            var resp = await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
            return resp > 0 ? id : null;
        }

        private Compumat ParseCompumat(SqlDataReader sqlDataReader)
        {
            Compumat compumat = new Compumat();
            compumat.Id = sqlDataReader.GetInt32("Id");
            compumat.Name = sqlDataReader["Name"].ToString()?.Trim();
            compumat.Latitude = sqlDataReader.GetDouble("Latitude");
            compumat.Longitude = sqlDataReader.GetDouble("Longitude");
            compumat.Type = (Compumat.CompumatType)sqlDataReader.GetInt32("Type");
            compumat.Status = sqlDataReader["Status"].ToString()?.Trim();
            return compumat;
        }
    }
}
