using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;using System.Data.SqlClient;
namespace Db_Connections
{
    class Db_Connection
    {
      //static string strConnection = @"Data Source=localhost;Initial Catalog=trackline;User Id=sa;Password=sqlserver";
        static string strConnection = @"Data Source=localhost\trackline;Initial Catalog=weapon_v1.0;User Id=sa;Password=sqlserver";
        //static string strConnection = @"Data Source=10.10.182.73\TRACKINGSYSTEM;Initial Catalog=weapon_v1.0;User Id=sa;Password=SqlServer15";
        public SqlConnection DbConn = new SqlConnection(strConnection);

        //Open Database Connection
        public void openConnection()
        {
            if (DbConn.State == ConnectionState.Closed)
            {
                DbConn.Open();
            }
        }

        //Close Database Connection
        public void closeConnection()
        {
            if (DbConn.State == ConnectionState.Open)
            {
                DbConn.Close();
                DbConn.Dispose();
            }
        }
        public string errors(string errmsg)
        {
            if (errmsg.Contains("12154") == true)
            {
                errmsg = "Database connection error...";

            }
            return errmsg;
        }
    }
}
