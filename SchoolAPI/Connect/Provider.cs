using System.Data.SqlClient;
using System.Data;

namespace SchoolAPI.Connect
{
    public class Provider
    {
		private string? ConnectionString()
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
			IConfiguration configuration = builder.Build();
			return configuration.GetValue<string>("ConnectionStrings:Default");
		}

		public SqlConnection connection { get; set; }

        public void Connect()
        {
            try
            {
                if(connection == null)
                    connection = new SqlConnection(ConnectionString());
                if(connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Disconnect()
        {
            try
            {
                if(connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int ExcuteNonQuery(CommandType cmdType, string strSql, params SqlParameter[] parameters)
        {
            int nrow = 0;
            try
            {
                Connect();
                
                SqlCommand cmd = new SqlCommand(strSql, connection);
                cmd.CommandText = strSql;
                cmd.CommandType = cmdType;
                if(parameters != null && parameters.Length > 0 )
                {
                    cmd.Parameters.AddRange(parameters);
                }
                nrow = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
            return nrow;
        }

        public DataTable Select(CommandType cmdType, string strSql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(strSql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                cmd.CommandText = strSql;
                cmd.CommandType = cmdType;  
                if(parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                adapter.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
            return dt;
        }
    }
}
