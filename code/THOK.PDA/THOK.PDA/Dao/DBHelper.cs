using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using THOK.PDA.Util;

namespace THOK.PDA.Dao
{
    public static class DBHelper
    {
        static string connectionString = new ConfigUtil().GetConfig("Connection")["ConnectionString"];
        static SqlConnection connection = null;
        public static SqlConnection Connection
        {
            get
            {
                try
                {
                    if (connection == null)
                    {
                        connection = new SqlConnection(connectionString);
                        connection.Open();
                    }
                    else if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    else if (connection.State == System.Data.ConnectionState.Broken)
                    {
                        connection.Close();
                        connection.Open();
                    }
                }
                catch (Exception)
                {
                    connection = null;
                }
                return connection;
            }
        }

        public static int ExecuteCommand(string safeSql)
        {
            SqlConnection con = Connection;

            SqlCommand cmd = new SqlCommand(safeSql, con);
            int result = cmd.ExecuteNonQuery();
            return result;
        }

        public static int ExecuteCommand(string sql, params SqlParameter[] values)
        {
            SqlConnection con = Connection;
            SqlCommand cmd = new SqlCommand(sql, con);
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    cmd.Parameters.Add(values[i]);
                }
            }
            return cmd.ExecuteNonQuery();
        }

        public static void ExecuteProcedure(string sql, params SqlParameter[] values)
        {
            SqlConnection con = Connection;
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    cmd.Parameters.Add(values[i]);
                }
            }
            cmd.ExecuteNonQuery();
        }

        public static int GetScalar(string safeSql)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }

        public static int GetScalar(string sql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    cmd.Parameters.Add(values[i]);
                }
            }
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }

        public static SqlDataReader GetReader(string safeSql)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public static SqlDataReader GetReader(string sql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    cmd.Parameters.Add(values[i]);
                }
            }
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public static DataTable GetDataTable(string safeSql)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable GetDataTable(string sql, params SqlParameter[] values)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, Connection);
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    cmd.Parameters.Add(values[i]);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds.Tables[0];
        }
    }
}
