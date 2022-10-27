using System.Data;
using System.Data.SqlClient;

namespace API.Repositories
{
    public class MapRepository
    {
        SqlConnection sqlConnection = new SqlConnection(API.ConnectionString.GetConnectionString());

        //read one from table
        public async Task<Map> GetMap(int id)
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM db.dbo.Site WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            Map result = new Map();
            while (sqlDataReader.Read())
            {
                result = ParseMap(sqlDataReader);
            }
            sqlConnection.Close();
            return result;
        }

        //read all from table
        public async Task<List<Map>> GetAllMaps()
        {
            List<Map> maps = new List<Map>();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM db.dbo.Site", sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            while (sqlDataReader.Read())
            {
                Map compumat = new Map();
                compumat = ParseMap(sqlDataReader);
                maps.Add(compumat);
            }
            sqlConnection.Close();
            return maps;
        }

        //create one in table
        public async Task<Map> CreateMap(Map map)
        {
            SqlDataAdapter sql = new SqlDataAdapter();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO db.dbo.Site (CampSiteName, Latitude, Longitude) OUTPUT INSERTED.* VALUES (@CampSiteName, @Latitude, @Longitude)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@CampSiteName", map.CampSiteName);
            sqlCommand.Parameters.AddWithValue("@Latitude", map.Latitude);
            sqlCommand.Parameters.AddWithValue("@Longitude", map.Longitude);
            sql.UpdateCommand = sqlCommand;
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            //connection lukker ikke hvis der opstår en fejl
            Map result = new Map();
            while (sqlDataReader.Read())
            {
                result = ParseMap(sqlDataReader);
            }
            sqlConnection.Close();
            return result;
        }

        //update one in table
        public async Task<Map> UpdateMap(Map map)
        {
            SqlDataAdapter sql = new SqlDataAdapter();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("UPDATE db.dbo.Site SET CampSiteName = @CampSiteName, Latitude = @Latitude, Longitude = @Longitude WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", map.Id);
            sqlCommand.Parameters.AddWithValue("@CampSiteName", map.CampSiteName);
            sqlCommand.Parameters.AddWithValue("@Latitude", map.Latitude);
            sqlCommand.Parameters.AddWithValue("@Longitude", map.Longitude);
            sqlCommand.ExecuteNonQuery();
            sql.UpdateCommand = sqlCommand;
            var resp = await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
            Map result = new Map();
            if (resp > 0)
            {
                result = await GetMap((int)map.Id);
            }
            return result;
        }

        //delete one from table
        public async Task<string> DeleteMap(string id)
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("DELETE FROM db.dbo.Site WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            var resp = await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
            return resp > 0 ? id : null;
        }


        private Map ParseMap(SqlDataReader sqlDataReader)
        {
            Map map = new Map();
            map.Id = sqlDataReader.GetInt32("Id");
            map.CampSiteName = sqlDataReader["CampSiteName"].ToString();
            map.Latitude = sqlDataReader.GetDouble("Latitude");
            map.Longitude = sqlDataReader.GetDouble("Longitude");
            return map;
        }

    }
}
