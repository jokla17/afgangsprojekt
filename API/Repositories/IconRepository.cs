using System.Data;
using System.Data.SqlClient;

namespace API.Repositories
{
    public class IconRepository
    {

        SqlConnection sqlConnection = new SqlConnection("Data Source=DESKTOP-P74EPR6\\SQLEXPRESS;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        //read all from table
        public List<Icon> GetAllIcons()
        {
            List<Icon> icons = new List<Icon>();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM db.dbo.Icon", sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Icon icon = new Icon();
                icon.Id = sqlDataReader.GetInt32("Id");
                icon.Name = sqlDataReader["Name"].ToString();
                icon.SvgIcon = sqlDataReader["SvgPath"].ToString();
                icons.Add(icon);
            }
            sqlConnection.Close();
            return icons;
        }

        //read one from table
        public Icon GetIcon(int id)
        {
            Icon icon = new Icon();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM db.dbo.Icon WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                icon.Id = sqlDataReader.GetInt32("Id");
                icon.Name = sqlDataReader["Name"].ToString();
                icon.SvgIcon = sqlDataReader["SvgPath"].ToString();
            }
            sqlConnection.Close();
            return icon;
        }

        //create one in table
        public void CreateIcon(Icon icon)
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO db.dbo.Icon (Id, Name, SvgPath) VALUES (@Id, @Name, @SvgPath)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", icon.Id);
            sqlCommand.Parameters.AddWithValue("@Name", icon.Name);
            sqlCommand.Parameters.AddWithValue("@SvgPath", icon.SvgIcon);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        //update one in table
        public void UpdateIcon(Icon icon)
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("UPDATE db.dbo.Icon SET Name = @Name, SvgPath = @SvgPath WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", icon.Id);
            sqlCommand.Parameters.AddWithValue("@Name", icon.Name);
            sqlCommand.Parameters.AddWithValue("@SvgPath", icon.SvgIcon);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }


        //delete one in table
        public void DeleteIcon(string id)
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("DELETE FROM db.dbo.Icon WHERE Id = @Id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        
    }
}
