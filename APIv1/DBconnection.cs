using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            String connectionString = @"Server=localhost; Database=webshop; User ID=API; Allow User Variables=True";
            conn = new MySqlConnection(connectionString);

            conn.Open();
        }
        
        
    }
}