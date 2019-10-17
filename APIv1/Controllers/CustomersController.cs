using APIv1.Data_Transfer_Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;

namespace APIv1.Controllers
{
    
    public class CustomersController : ApiController
    {
        // GET api/v1/<controller>
        // GETs customers from database 
        [HttpGet]
        [Route("api/v1/customers")]
        public List<CustomerDto> GetCustomersFromDatabase()
        {
            // declare variables
            String SQLString = "SELECT * FROM customers";
            DataTable dataTableCustomers = new DataTable();
            DBconnection dbConnection = new DBconnection();

            List<CustomerDto> customersFromDatabase = new List<CustomerDto>();
                  

            // open connection to database
            dbConnection.openConnection();

            // create data adapter and fill dataTableCustomers with data from data adapter (customer data) 
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(SQLString, dbConnection.conn);
            dataAdapter.Fill(dataTableCustomers);


            for (int i = 0; i < dataTableCustomers.Rows.Count; i++)
            {
                CustomerDto customerDto = new CustomerDto();
                //customerDto.wp_user_id = (int?)dataTableCustomers.Rows[i]["wp_user_id"];
                customerDto.customer_id = dataTableCustomers.Rows[i]["customer_id"].ToString();

                try
                {
                    customerDto.url = "http://localhost/api/v1/customers/" + dataTableCustomers.Rows[i]["customer_id"];
                    customerDto.username = dataTableCustomers.Rows[i]["username"].ToString();
                    customerDto.first_name = dataTableCustomers.Rows[i]["first_name"].ToString();
                    customerDto.last_name = dataTableCustomers.Rows[i]["last_name"].ToString();
                    customerDto.email = dataTableCustomers.Rows[i]["email"].ToString();
                    customerDto.phone = dataTableCustomers.Rows[i]["phone"].ToString();
                    customerDto.password = dataTableCustomers.Rows[i]["password"].ToString();
                    customerDto.role = dataTableCustomers.Rows[i]["role"].ToString();
                }
                catch
                {                    
                    continue;
                }

                try
                {
                    customerDto.billing["first_name"] = dataTableCustomers.Rows[i]["first_name_billing"].ToString();
                    customerDto.billing["last_name"] = dataTableCustomers.Rows[i]["last_name_billing"].ToString();
                    customerDto.billing["company"] = dataTableCustomers.Rows[i]["company_billing"].ToString();
                    customerDto.billing["address"] = dataTableCustomers.Rows[i]["address_billing"].ToString();
                    customerDto.billing["city"] = dataTableCustomers.Rows[i]["city_billing"].ToString();
                    customerDto.billing["post_code"] = dataTableCustomers.Rows[i]["post_code_billing"].ToString();
                    customerDto.billing["country"] = dataTableCustomers.Rows[i]["country_billing"].ToString();
                    customerDto.billing["email"] = dataTableCustomers.Rows[i]["email_billing"].ToString();
                }
                catch
                {
                    customerDto.billing = null;
                }

                try
                {
                    customerDto.shipping["first_name"] = dataTableCustomers.Rows[i]["first_name_shipping"].ToString();
                    customerDto.shipping["last_name"] = dataTableCustomers.Rows[i]["last_name_shipping"].ToString();
                    customerDto.shipping["company"] = dataTableCustomers.Rows[i]["company_shipping"].ToString();
                    customerDto.shipping["address"] = dataTableCustomers.Rows[i]["address_shipping"].ToString();
                    customerDto.shipping["city"] = dataTableCustomers.Rows[i]["city_shipping"].ToString();
                    customerDto.shipping["post_code"] = dataTableCustomers.Rows[i]["post_code_shipping"].ToString();
                    customerDto.shipping["country"] = dataTableCustomers.Rows[i]["country_shipping"].ToString();
                }
                catch
                {
                    customerDto.shipping = null;
                }
                
                customersFromDatabase.Add(customerDto);

            }


            dbConnection.conn.Close();           

            return customersFromDatabase;
        }

        // GET api/v1/<controller>/5
        [HttpGet]
        [Route("api/v1/customers/{id}")]
        public CustomerDto GetCustomerFromDatabase(int id)
        {
            // declare variables
            CustomerDto customerDto = new CustomerDto(); 
            String SQLString = "SELECT * FROM customers";
            DataTable dataTableCustomers = new DataTable();
            DBconnection dbConnection = new DBconnection();

            List<CustomerDto> customersFromDatabase = new List<CustomerDto>();


            // open connection to database
            dbConnection.openConnection();

            // create data adapter and fill dataTableCustomers with data from data adapter (customer data) 
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(SQLString, dbConnection.conn);
            dataAdapter.Fill(dataTableCustomers);


            for (int i = 0; i < dataTableCustomers.Rows.Count; i++)
            {
                if ((int)dataTableCustomers.Rows[i]["customer_id"] == id)
                {
                    
                    customerDto.wp_user_id = (int)dataTableCustomers.Rows[i]["wp_user_id"];
                    customerDto.customer_id = dataTableCustomers.Rows[i]["customer_id"].ToString();

                    try
                    {
                        customerDto.url = "http://localhost/api/v1/customers/" + dataTableCustomers.Rows[i]["customer_id"];
                        customerDto.username = dataTableCustomers.Rows[i]["username"].ToString();
                        customerDto.first_name = dataTableCustomers.Rows[i]["first_name"].ToString();
                        customerDto.last_name = dataTableCustomers.Rows[i]["last_name"].ToString();
                        customerDto.email = dataTableCustomers.Rows[i]["email"].ToString();
                        customerDto.phone = dataTableCustomers.Rows[i]["phone"].ToString();
                        customerDto.password = dataTableCustomers.Rows[i]["password"].ToString();
                        customerDto.role = dataTableCustomers.Rows[i]["role"].ToString();
                    }
                    catch
                    {
                        continue;
                    }

                    try
                    {
                        customerDto.billing["first_name"] = dataTableCustomers.Rows[i]["first_name_billing"].ToString();
                        customerDto.billing["last_name"] = dataTableCustomers.Rows[i]["last_name_billing"].ToString();
                        customerDto.billing["company"] = dataTableCustomers.Rows[i]["company_billing"].ToString();
                        customerDto.billing["address"] = dataTableCustomers.Rows[i]["address_billing"].ToString();
                        customerDto.billing["city"] = dataTableCustomers.Rows[i]["city_billing"].ToString();
                        customerDto.billing["post_code"] = dataTableCustomers.Rows[i]["post_code_billing"].ToString();
                        customerDto.billing["country"] = dataTableCustomers.Rows[i]["country_billing"].ToString();
                        customerDto.billing["email"] = dataTableCustomers.Rows[i]["email_billing"].ToString();
                    }
                    catch
                    {
                        customerDto.billing = null;
                    }

                    try
                    {
                        customerDto.shipping["first_name"] = dataTableCustomers.Rows[i]["first_name_shipping"].ToString();
                        customerDto.shipping["last_name"] = dataTableCustomers.Rows[i]["last_name_shipping"].ToString();
                        customerDto.shipping["company"] = dataTableCustomers.Rows[i]["company_shipping"].ToString();
                        customerDto.shipping["address"] = dataTableCustomers.Rows[i]["address_shipping"].ToString();
                        customerDto.shipping["city"] = dataTableCustomers.Rows[i]["city_shipping"].ToString();
                        customerDto.shipping["post_code"] = dataTableCustomers.Rows[i]["post_code_shipping"].ToString();
                        customerDto.shipping["country"] = dataTableCustomers.Rows[i]["country_shipping"].ToString();
                    }
                    catch
                    {
                        customerDto.shipping = null;
                    }
                    dbConnection.conn.Close();
                    return customerDto;
                }

                
                else
                {
                    if (i == dataTableCustomers.Rows.Count -1)
                    {
                        dbConnection.conn.Close();
                        return customerDto = null;
                    }               
                    
                }

            }          
                return customerDto = null;
        }

        //POST api/v1/<controller>
        //gets customers from POST request and writes them into database
        [HttpPost]
        [Route("api/v1/customers")]
        public List<CustomerDto> PostCustomers([FromBody] JArray content)
        {
            
            int numberOfCustomers = 0;
            List<int> customersAlreadyInDatabase = new List<int>();
            
            List<CustomerDto> customersCreated = new List<CustomerDto>();  


            //open DBconnection
            DBconnection dbConnection = new DBconnection();
            dbConnection.openConnection();

            foreach (JObject customer in content)
            {
                CustomerDto customerDto = new CustomerDto(customer);
                //creates DB entry for each Object                   
                dbConnection.SQLString = "INSERT INTO customers (wp_user_id, username, first_name, last_name, email, phone, " +
                    "first_name_billing, last_name_billing, company_billing, address_billing, city_billing, post_code_billing, country_billing, email_billing," +
                    "first_name_shipping, last_name_shipping, company_shipping, address_shipping, city_shipping, post_code_shipping, country_shipping)" +
                    "VALUES(@wp_user_id, @username, @first_name, @last_name, @email, @phone, " +
                    "@first_name_billing, @last_name_billing, @company_billing, @address_billing, @city_billing, @post_code_billing, @country_billing, @email_billing," +
                    "@first_name_shipping, @last_name_shipping, @company_shipping, @address_shipping, @city_shipping, @post_code_shipping, @country_shipping)";
                dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);

                dbConnection.com.Parameters.AddWithValue("@wp_user_id", customerDto.wp_user_id);
                dbConnection.com.Parameters.AddWithValue("@username", customerDto.username);
                dbConnection.com.Parameters.AddWithValue("@first_name", customerDto.first_name);
                dbConnection.com.Parameters.AddWithValue("@last_name", customerDto.last_name);
                dbConnection.com.Parameters.AddWithValue("@email", customerDto.email);
                dbConnection.com.Parameters.AddWithValue("@phone", customerDto.phone);

                try
                {
                    dbConnection.com.Parameters.AddWithValue("@first_name_billing", customerDto.billing["first_name"]);
                    dbConnection.com.Parameters.AddWithValue("@last_name_billing", customerDto.billing["last_name"]);
                    dbConnection.com.Parameters.AddWithValue("@company_billing", customerDto.billing["company"]);
                    dbConnection.com.Parameters.AddWithValue("@address_billing", customerDto.billing["address_1"]);
                    dbConnection.com.Parameters.AddWithValue("@city_billing", customerDto.billing["city"]);
                    dbConnection.com.Parameters.AddWithValue("@post_code_billing", customerDto.billing["postcode"]);
                    dbConnection.com.Parameters.AddWithValue("@country_billing", customerDto.billing["country"]);
                    dbConnection.com.Parameters.AddWithValue("@email_billing", customerDto.billing["email"]);
                }
                catch 
                {

                    dbConnection.com.Parameters.AddWithValue("@billing", null);
                }

                try
                {
                    dbConnection.com.Parameters.AddWithValue("@first_name_shipping", customerDto.shipping["first_name"]);
                    dbConnection.com.Parameters.AddWithValue("@last_name_shipping", customerDto.shipping["last_name"]);
                    dbConnection.com.Parameters.AddWithValue("@company_shipping", customerDto.shipping["company"]);
                    dbConnection.com.Parameters.AddWithValue("@address_shipping", customerDto.shipping["address_1"]);
                    dbConnection.com.Parameters.AddWithValue("@city_shipping", customerDto.shipping["city"]);
                    dbConnection.com.Parameters.AddWithValue("@post_code_shipping", customerDto.shipping["postcode"]);
                    dbConnection.com.Parameters.AddWithValue("@country_shipping", customerDto.shipping["country"]);
                }
                catch 
                {

                    dbConnection.com.Parameters.AddWithValue("@shipping", null);
                }

                //dbConnection.com.Dispose();

                try
                {
                    numberOfCustomers += dbConnection.com.ExecuteNonQuery();
                    customerDto.url = @"http://localhost:51074/api/v1/customers/" + customerDto.wp_user_id;
                    customersCreated.Add(customerDto);
                }
                catch 
                {  
                    continue;
                }

            }
            for (int i = 0; i < customersAlreadyInDatabase.Count; i++)
            {
                content.RemoveAt(customersAlreadyInDatabase[i]);
            }

            dbConnection.conn.Close();
            return customersCreated;
        }    

        // POST api/v1/<controller>/<source>
        // customers created in webshop get sent to database via webhook
        [HttpPost]
        [Route("api/v1/customers/webshop")]
        public CustomerDto PostCustomerToDatabase(CustomerDto customerDto)
        {               
            
            //open DB connection
            DBconnection dbConnection = new DBconnection();
                dbConnection.openConnection();

                // enter customer into DB
                dbConnection.SQLString = "INSERT INTO customers (wp_user_id, username, first_name, last_name, email, phone, " +
                            "first_name_billing, last_name_billing, company_billing, address_billing, city_billing, post_code_billing, country_billing, email_billing," +
                            "first_name_shipping, last_name_shipping, company_shipping, address_shipping, city_shipping, post_code_shipping, country_shipping)" +
                            "VALUES(@wp_user_id, @username, @first_name, @last_name, @email, @phone, " +
                            "@first_name_billing, @last_name_billing, @company_billing, @address_billing, @city_billing, @post_code_billing, @country_billing, @email_billing," +
                            "@first_name_shipping, @last_name_shipping, @company_shipping, @address_shipping, @city_shipping, @post_code_shipping, @country_shipping)";
                dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);

                dbConnection.com.Parameters.AddWithValue("@wp_user_id", customerDto.wp_user_id);

            try
            {
                dbConnection.com.Parameters.AddWithValue("@username", customerDto.username);

                dbConnection.com.Parameters.AddWithValue("@first_name", customerDto.first_name);
                dbConnection.com.Parameters.AddWithValue("@last_name", customerDto.last_name);
                dbConnection.com.Parameters.AddWithValue("@email", customerDto.email);
                dbConnection.com.Parameters.AddWithValue("@phone", customerDto.phone);

                try
                {
                    dbConnection.com.Parameters.AddWithValue("@first_name_billing", customerDto.billing["first_name"]);
                    dbConnection.com.Parameters.AddWithValue("@last_name_billing", customerDto.billing["last_name"]);
                    dbConnection.com.Parameters.AddWithValue("@company_billing", customerDto.billing["company"]);
                    dbConnection.com.Parameters.AddWithValue("@address_billing", customerDto.billing["address_1"]);
                    dbConnection.com.Parameters.AddWithValue("@city_billing", customerDto.billing["city"]);
                    dbConnection.com.Parameters.AddWithValue("@post_code_billing", customerDto.billing["postcode"]);
                    dbConnection.com.Parameters.AddWithValue("@country_billing", customerDto.billing["country"]);
                    dbConnection.com.Parameters.AddWithValue("@email_billing", customerDto.billing["email"]);
                }
                catch
                {

                    dbConnection.com.Parameters.AddWithValue("@billing", null);
                }

                try
                {
                    dbConnection.com.Parameters.AddWithValue("@first_name_shipping", customerDto.shipping["first_name"]);
                    dbConnection.com.Parameters.AddWithValue("@last_name_shipping", customerDto.shipping["last_name"]);
                    dbConnection.com.Parameters.AddWithValue("@company_shipping", customerDto.shipping["company"]);
                    dbConnection.com.Parameters.AddWithValue("@address_shipping", customerDto.shipping["address_1"]);
                    dbConnection.com.Parameters.AddWithValue("@city_shipping", customerDto.shipping["city"]);
                    dbConnection.com.Parameters.AddWithValue("@post_code_shipping", customerDto.shipping["postcode"]);
                    dbConnection.com.Parameters.AddWithValue("@country_shipping", customerDto.shipping["country"]);
                }
                catch
                {

                    dbConnection.com.Parameters.AddWithValue("@shipping", null);
                }

                dbConnection.com.ExecuteNonQuery();
                dbConnection.com.Dispose();
                dbConnection.conn.Close();
            }
            catch
            {
                UpdateCustomer(customerDto);                
            }

            customerDto.url = @"http://localhost:51074/api/v1/customers/" + customerDto.wp_user_id;
            return customerDto;
        }
           

        // POST api/v1/<controller>/update/
        // gets post request from webshop when a customer is updated and updates customer in db
        [HttpPost]
        [Route("api/v1/customers/update")]
        public CustomerDto UpdateCustomer(CustomerDto customerDto)
        {
            
            //open DB connection
            DBconnection dbConnection = new DBconnection();
            dbConnection.openConnection();

            // enter customer into DB
            dbConnection.SQLString = "UPDATE customers SET wp_user_id = @wp_user_id, first_name = @first_name, last_name = @Last_name, email = @email, phone = @phone, " +
                    "first_name_billing = @first_name_billing, last_name_billing = @Last_name_billing, company_billing = @company_billing, address_billing = @address_billing, city_billing = @city_billing, post_code_billing = @post_code_billing, country_billing = @country_billing, email_billing = @email_billing, " +
                    "first_name_shipping = @first_name_shipping, last_name_shipping = @Last_name_shipping, company_shipping = @company_shipping, address_shipping = @address_shipping, city_shipping = @city_shipping, post_code_shipping = @post_code_shipping, country_shipping = @country_shipping " +
                    "WHERE username = @username";

            
            dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);

            dbConnection.com.Parameters.AddWithValue("@wp_user_id", customerDto.wp_user_id);           
            dbConnection.com.Parameters.AddWithValue("@username", customerDto.username);           
            dbConnection.com.Parameters.AddWithValue("@first_name", customerDto.first_name);
            dbConnection.com.Parameters.AddWithValue("@last_name", customerDto.last_name);
            dbConnection.com.Parameters.AddWithValue("@email", customerDto.email);
            dbConnection.com.Parameters.AddWithValue("@phone", customerDto.phone);

            dbConnection.com.Parameters.AddWithValue("@first_name_billing", customerDto.billing["first_name"]);
            dbConnection.com.Parameters.AddWithValue("@last_name_billing", customerDto.billing["last_name"]);
            dbConnection.com.Parameters.AddWithValue("@company_billing", customerDto.billing["company"]);
            dbConnection.com.Parameters.AddWithValue("@address_billing", customerDto.billing["address_1"]);
            dbConnection.com.Parameters.AddWithValue("@city_billing", customerDto.billing["city"]);
            dbConnection.com.Parameters.AddWithValue("@post_code_billing", customerDto.billing["postcode"]);
            dbConnection.com.Parameters.AddWithValue("@country_billing", customerDto.billing["country"]);
            dbConnection.com.Parameters.AddWithValue("@email_billing", customerDto.billing["email"]);

            dbConnection.com.Parameters.AddWithValue("@first_name_shipping", customerDto.shipping["first_name"]);
            dbConnection.com.Parameters.AddWithValue("@last_name_shipping", customerDto.shipping["last_name"]);
            dbConnection.com.Parameters.AddWithValue("@company_shipping", customerDto.shipping["company"]);
            dbConnection.com.Parameters.AddWithValue("@address_shipping", customerDto.shipping["address_1"]);
            dbConnection.com.Parameters.AddWithValue("@city_shipping", customerDto.shipping["city"]);
            dbConnection.com.Parameters.AddWithValue("@post_code_shipping", customerDto.shipping["postcode"]);
            dbConnection.com.Parameters.AddWithValue("@country_shipping", customerDto.shipping["country"]);

            //execute nonQuery and close connection
            try
            {
                dbConnection.com.ExecuteNonQuery();
                
            }
            catch 
            {
                customerDto = null;               
            }

            dbConnection.com.Dispose();
            dbConnection.conn.Close();            

            return customerDto;
        }

        // DELETE api/v1/<controller>/delete
        // gets POST request from webshop when customer is deleted and deletes customer from db
        [HttpPost]
        [Route ("api/v1/customers/delete")]
        public CustomerDto DeleteCustomer(CustomerDto customerDto)
        {
            
            //open DB connection
            DBconnection dbConnection = new DBconnection();
            dbConnection.openConnection();

            // delete customer from DB
            dbConnection.SQLString = "UPDATE customers SET customer_active = FALSE WHERE wp_user_id = @wp_user_id";

            dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);

            dbConnection.com.Parameters.AddWithValue("@wp_user_id", customerDto.wp_user_id);

            // execute nonQuery and close connection
            try
            {
                dbConnection.com.ExecuteNonQuery();
                customerDto.url = @"http://localhost:51074/api/v1/customers/" + customerDto.wp_user_id;
            }
            catch 
            {

                customerDto = null;
            }
            dbConnection.com.Dispose();
            dbConnection.conn.Close();

            return customerDto;
        }
    }
}