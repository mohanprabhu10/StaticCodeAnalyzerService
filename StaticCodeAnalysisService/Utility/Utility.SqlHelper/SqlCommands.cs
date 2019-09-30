using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Utility.SqlHelper
{
    public static class SqlCommands
    {
        public static void OpenConnection(SqlConnection con)
        {
            try
            {
                con.Open();
            }
            catch
            {
                throw new FaultException("Db connection is not valid");
            }
        }


        public static SqlDataReader ExecuteRead(SqlConnection con, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {
                    return cmd.ExecuteReader();
                }
                catch
                {
                    throw new FaultException("DB read is invalid");
                }
            }


        }

        public static bool ExecuteCommand(string query, string connectionString)
        {
            int noRows;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    OpenConnection(con);
                    SqlCommand cmd = new SqlCommand(query, con);
                    noRows = cmd.ExecuteNonQuery();

                }
                catch
                {
                    return false;
                    //"throw new FaultException("Database operation is invalid");"
                }
            }

            return (noRows > 0);
        }

        public static bool Exists(string connectionString, string query)
        {
            bool isExists = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                OpenConnection(con);
                SqlDataReader reader = SqlCommands.ExecuteRead(con, query);
                if (reader.HasRows)
                {
                    isExists = true;
                }
                reader.Close();
            }
            return isExists;
        }


    }
}
