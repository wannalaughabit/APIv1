using APIv1.Data_Transfer_Objects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIv1.Controllers
{
    public class OrdersController : ApiController
    {
        // POST api/v1/<controller>/<source>
        // order created in webshop get sent to database via webhook
        [HttpPost]
        [Route("api/v1/orders/webshop")]
        public OrderDto PostOrderToDatabase(OrderDto orderDto)
        {
            // get customer id from table customers

            // declare variables
            //String id_customer = null;
            int id_customer = 0;
            //String SQLString = "SELECT customer_id FROM customers WHERE wp_user_id = @wp_user_id";
            String SQLString = "SELECT * FROM customers";
            DataTable dataTableCustomers = new DataTable();            
            DBconnection dbConnectionSelect = new DBconnection();
            //dbConnectionSelect.com = new MySqlCommand(SQLString, dbConnectionSelect.conn);
            // open connection to database
            dbConnectionSelect.openConnection();
            //dbConnectionSelect.com.Parameters.AddWithValue("@wp_user_id", orderDto.wp_customer_id);
            //id_customer = dbConnectionSelect.com.ExecuteReader().ToString();
           

            // create data adapter and fill dataTableCustomers with data from data adapter (customer data) 
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(SQLString, dbConnectionSelect.conn);
            dataAdapter.Fill(dataTableCustomers);

            for (int i = 0; i < dataTableCustomers.Rows.Count; i++)
            {
                if ((int)dataTableCustomers.Rows[i]["wp_user_id"] == orderDto.wp_customer_id)
                {
                    id_customer = (int)dataTableCustomers.Rows[i]["customer_id"];
                }
            }


            dbConnectionSelect.conn.Close();

            // enter order into DB
            //open DB connection
            DBconnection dbConnection = new DBconnection();
            dbConnection.openConnection();
            dbConnection.SQLString = "INSERT INTO orders (wp_order_id, id_customer, date, total)" +                       
                        "VALUES(@wp_order_id, @id_customer, @date, @total)";            
                        
            dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);

            dbConnection.com.Parameters.AddWithValue("@wp_order_id", orderDto.wp_order_id);
            dbConnection.com.Parameters.AddWithValue("@id_customer", id_customer);
            dbConnection.com.Parameters.AddWithValue("@date", orderDto.date);                
            dbConnection.com.Parameters.AddWithValue("@total", orderDto.total);                
                            

            dbConnection.com.ExecuteNonQuery();
            dbConnection.com.Dispose();
            dbConnection.conn.Close();
            
           

            orderDto.url = @"http://localhost:51074/api/v1/customers/" + orderDto.wp_order_id;
            return orderDto;
        }
    }
}
