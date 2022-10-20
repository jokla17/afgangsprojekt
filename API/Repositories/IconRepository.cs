using System.Data.SqlClient;

namespace API.Repositories
{
    public class IconRepository
    {

        SqlConnection sqlConnection = new SqlConnection("Data Source=DESKTOP-P74EPR6\\SQLEXPRESS;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        //read all from table
        public List<Icon> GetAll()
        {
            List<Icon> icons = new List<Icon>();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM db.dbo.Icon", sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Icon icon = new Icon();
                icon.Id = sqlDataReader["Id"].ToString();
                icon.Name = sqlDataReader["Name"].ToString();
                icon.SvgIcon = sqlDataReader["SvgPath"].ToString().TrimEnd();
                icons.Add(icon);
            }
            sqlConnection.Close();
            return icons;
        }
    }
}
