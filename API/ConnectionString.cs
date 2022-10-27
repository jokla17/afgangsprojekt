using System.Diagnostics;

namespace API {
    public static class ConnectionString {

        

        public static string GetConnectionString() {
            string machineName = System.Environment.MachineName;
            string _connectionString = $"Data Source={machineName}\\SQLEXPRESS;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            return _connectionString;
        }

    }
}
