using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace APIv1
{
    public class DBconnection
    {
        public MySqlConnection conn = new MySqlConnection();
        public MySqlCommand com;
        public String SQLString;

        public DBconnection()
        {

        }

        public void openConnection()
        {
            //open DB connection
            
            String connectionString = File.ReadAllText(@"D:\connectionString.txt");
            conn = new MySqlConnection(connectionString);

            conn.Open();
        }
        
        
    }
}