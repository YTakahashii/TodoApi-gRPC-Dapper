using System;
using System.Data.SqlClient;

namespace TodoApi_gRPC_Dapper.Models.SqlHandler
{
    public class SqlHandler
    {
        public SqlHandler()
        {
        }

        public SqlConnection GetConnection()
        {
            var connectionString = "Data Source=127.0.0.1;Initial Catalog=TodoApi_Dapper;Connect Timeout=60;Persist Security Info=True;User ID=sa;Password=Password0123";
            var connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
