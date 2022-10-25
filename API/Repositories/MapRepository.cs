using System.Data;
using System.Data.SqlClient;

namespace API.Repositories
{
    public class MapRepository
    {

        SqlConnection sqlConnection = new SqlConnection("Data Source=DESKTOP-P74EPR6\\SQLEXPRESS;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        //read one from table
        public async Task<Map> GetMap(string id)
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
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO db.dbo.Site (CampSiteName, Longitude, Latitude) OUTPUT INSERTED.* VALUES (@CampSiteName, @Longitude, @Latitude)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@CampSiteName", map.CampSiteName);
            sqlCommand.Parameters.AddWithValue("@Longitude", map.Longitude);
            sqlCommand.Parameters.AddWithValue("@Latitude", map.Latitude);
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
            SqlCommand sqlCommand = new SqlCommand("UPDATE db.dbo.Site SET CampSiteName = @CampSiteName, Longitude = @Longitude, Latitude = @Latitude WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", map.Id);
            sqlCommand.Parameters.AddWithValue("@CampSiteName", map.CampSiteName);
            sqlCommand.Parameters.AddWithValue("@Longitude", map.Longitude);
            sqlCommand.Parameters.AddWithValue("@Latitude", map.Latitude);
            sqlCommand.ExecuteNonQuery();
            sql.UpdateCommand = sqlCommand;
            var resp = await sqlCommand.ExecuteNonQueryAsync();
            sqlConnection.Close();
            Map result = new Map();
            if (resp > 0)
            {
                result = await GetMap(map.Id);
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
            map.Id = sqlDataReader["Id"].ToString();
            map.CampSiteName = sqlDataReader["CampSiteName"].ToString();
            map.Longitude = sqlDataReader.GetDouble("Longitude");
            map.Latitude = sqlDataReader.GetDouble("Latitude");
            return map;
        }

    }
}
