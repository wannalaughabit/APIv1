using APIv1.Data_Transfer_Objects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            
            int id_customer = 0;            
            String SQLString = "SELECT * FROM customers";
            DataTable dataTableCustomers = new DataTable();            
            DBconnection dbConnectionSelect = new DBconnection();            
            dbConnectionSelect.openConnection();          
           

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
                        "VALUES(@wp_order_id, @id_customer, @date, @total);";            
                        
            dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);

            dbConnection.com.Parameters.AddWithValue("@wp_order_id", orderDto.wp_order_id);
            dbConnection.com.Parameters.AddWithValue("@id_customer", id_customer);
            dbConnection.com.Parameters.AddWithValue("@date", orderDto.date);                
            dbConnection.com.Parameters.AddWithValue("@total", orderDto.total);


            try
            {
                dbConnection.com.ExecuteNonQuery();
            }
            catch
            {
                return orderDto;

            }
            dbConnection.com.Dispose();
            dbConnection.conn.Close();

            //PostItemsToOrder_Items(orderDto);
            orderDto.url = @"http://localhost:51074/api/v1/customers/" + orderDto.wp_order_id;
            return orderDto;
        }
/*
        //order id is not identical with db order id
        // when order is created in webshop insert into orders_items
        [HttpPost]
        [Route("api/v1/orders_items/webshop")]
        public void PostItemsToOrder_Items(OrderDto orderDto)
        {
            // get item id from table customers

            // declare variables
            int i = 0;
            int? order_id = null;
            int? item_id = null;
            MySqlDataReader reader;
            DBconnection dbConnection = new DBconnection();
            dbConnection.openConnection();
            DBconnection dbConnectionWrite = new DBconnection();
            //try
            //{
            foreach (var item in orderDto.line_items)
                {
                //get order id from db
                dbConnection.SQLString = "SELECT order_id FROM orders WHERE wp_order_id = @wp_order_id";
                dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);
                dbConnection.com.Parameters.AddWithValue("@wp_order_id", orderDto.wp_order_id.ToString());
                reader = dbConnection.com.ExecuteReader();

                while (reader.Read())
                {
                    order_id = (int)reader[0];
                }
                    
                reader.Close();
                

                //Get item id from db
                dbConnection.SQLString = "SELECT item_id FROM items WHERE wp_item_id = @wp_item_id";
                dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);
                dbConnection.com.Parameters.AddWithValue("@wp_item_id", orderDto.line_items[i]["product_id"]);
                reader = dbConnection.com.ExecuteReader();

                while (reader.Read())
                {
                    item_id = (int)reader[0];
                }
                reader.Close();

                //write into orders_items
               
                dbConnectionWrite.openConnection();
                dbConnectionWrite.SQLString = "INSERT INTO orders_items (id_order, id_item, quantity)" +
                                "VALUES(@id_order, @id_item, @quantity);";

                    dbConnectionWrite.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);

                    dbConnectionWrite.com.Parameters.AddWithValue("@quantity", orderDto.line_items[i]["quantity"]);
                    dbConnectionWrite.com.Parameters.AddWithValue("@id_order", order_id);
                    dbConnectionWrite.com.Parameters.AddWithValue("@id_item", item_id);
                    i++;
                    //try
                    //{
                        dbConnectionWrite.com.ExecuteNonQuery();
                    //}
                    //catch
                    //{
                    //    continue;
                    //}

                }
            //}
            //catch 
            //{

            //    return orderDto;
            //}
            dbConnection.com.Dispose();
            dbConnection.conn.Close();
            dbConnectionWrite.com.Dispose();
            dbConnectionWrite.conn.Close();


            orderDto.url = @"http://localhost:51074/api/v1/customers/" + orderDto.wp_order_id;
            
        }
 */
    }

}
