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

namespace APIv1.Controllers
{
    
    public class CustomersController : ApiController
    {
        // GET api/<controller>
        // gets customers from webshop and writes them into database
        [HttpGet]
        [Route ("api/v1/customers/webshop")]
        public string GetCustomers()
        {
            int numberOfCustomers = 0;
            int NumberOfcustomersAlreadyInDatabase = 0;
            string IdsOfExistingCustomers = "";
            string messageExistingCustomers = "";
            string returnMessage;
            List<string> exception = new List<string>();

            //open DBconnection
            DBconnection dbConnection = new DBconnection();
            dbConnection.openConnection();                
            
            

            JObject obj = null;
            CustomerDto customerDto;
            WebReq request = new WebReq();
            JsonTextReader json = request.createGetRequest("customers");
            

            //loop through objects to create a CustomerDto for each and pass it to CreateCustomer
            while (json.Read())
            {
                if (json.TokenType == JsonToken.StartObject)
                {
                    obj = JObject.Load(json);
                    customerDto = new CustomerDto(obj);                                        
                    
                    //creates DB entry for each Object                   
                    dbConnection.SQLString = "INSERT INTO customers (wp_user_id, username, first_name, last_name, email, phone_number, " +
                        "first_name_billing, last_name_billing, company_billing, address_billing, city_billing, post_code_billing, country_billing, email_billing," +
                        "first_name_shipping, last_name_shipping, company_shipping, address_shipping, city_shipping, post_code_shipping, country_shipping)" +
                        "VALUES(@wp_user_id, @username, @first_name, @last_name, @email, @phone_number, " +
                        "@first_name_billing, @last_name_billing, @company_billing, @address_billing, @city_billing, @post_code_billing, @country_billing, @email_billing," +
                        "@first_name_shipping, @last_name_shipping, @company_shipping, @address_shipping, @city_shipping, @post_code_shipping, @country_shipping)";
                    dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);                    
                    
                    dbConnection.com.Parameters.AddWithValue("@wp_user_id", customerDto.id);
                    dbConnection.com.Parameters.AddWithValue("@username", customerDto.username);
                    dbConnection.com.Parameters.AddWithValue("@first_name", customerDto.first_name);
                    dbConnection.com.Parameters.AddWithValue("@last_name", customerDto.last_name);
                    dbConnection.com.Parameters.AddWithValue("@email", customerDto.email);
                    dbConnection.com.Parameters.AddWithValue("@phone_number", customerDto.phone_number);
                    
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
                    
                    dbConnection.com.Dispose();

                    try
                    {
                        numberOfCustomers += dbConnection.com.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {       
                        // add exception message to list so I can return the IDs of customers already in DB
                        exception.Add(ex.Message);
                        NumberOfcustomersAlreadyInDatabase += 1;
                        continue;
                    }
                }                
            }

            dbConnection.conn.Close();
            request.stream.Close();
            request.response.Close();            

            //loop through array of exception messages and extract IDs
            for (int i = 0; i < exception.Count; i++)
            {
               IdsOfExistingCustomers = IdsOfExistingCustomers + ", " + string.Join("", exception[i].ToCharArray().Where(Char.IsDigit));                
            }

            //switch to display correct message for customers already in database
            switch (exception.Count)
            {
                case 0:
                    messageExistingCustomers = NumberOfcustomersAlreadyInDatabase + " customers with the IDs" + IdsOfExistingCustomers + " were already in the database.";
                    break;
                case 1:
                    messageExistingCustomers = NumberOfcustomersAlreadyInDatabase + " customer with the ID" + IdsOfExistingCustomers + " was already in the database.";
                    break;
                default:
                    messageExistingCustomers = NumberOfcustomersAlreadyInDatabase + " customers with the IDs" + IdsOfExistingCustomers + " were already in the database.";
                    break;
            }

            //switch to display correct message
            switch (numberOfCustomers)
            {
                case 0:
                    returnMessage = messageExistingCustomers + " No customers were added to the database.";
                    break;
                case 1:
                    returnMessage = messageExistingCustomers + NumberOfcustomersAlreadyInDatabase + " customers were already in the database." + "One customer was added to the database.";
                    break;
                default:
                    returnMessage = messageExistingCustomers + numberOfCustomers + " customers were added to the database.";
                    break;
            }
            
            return returnMessage;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>/<source>
        // customers created in webshop get sent to database via POST request/webhook
        [HttpPost]
        [Route("api/v1/customers/webshop")]
        public int PostCustomerToDatabase(CustomerDto customerDto)
        {            
                //open DB connection
                DBconnection dbConnection = new DBconnection();
                dbConnection.openConnection();

                // enter customer into DB
                dbConnection.SQLString = "INSERT INTO customers (wp_user_id, username, first_name, last_name, email, phone_number, " +
                            "first_name_billing, last_name_billing, company_billing, address_billing, city_billing, post_code_billing, country_billing, email_billing," +
                            "first_name_shipping, last_name_shipping, company_shipping, address_shipping, city_shipping, post_code_shipping, country_shipping)" +
                            "VALUES(@wp_user_id, @username, @first_name, @last_name, @email, @phone_number, " +
                            "@first_name_billing, @last_name_billing, @company_billing, @address_billing, @city_billing, @post_code_billing, @country_billing, @email_billing," +
                            "@first_name_shipping, @last_name_shipping, @company_shipping, @address_shipping, @city_shipping, @post_code_shipping, @country_shipping)";
                dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);

                dbConnection.com.Parameters.AddWithValue("@wp_user_id", customerDto.id);
                dbConnection.com.Parameters.AddWithValue("@username", customerDto.username);
                dbConnection.com.Parameters.AddWithValue("@first_name", customerDto.first_name);
                dbConnection.com.Parameters.AddWithValue("@last_name", customerDto.last_name);
                dbConnection.com.Parameters.AddWithValue("@email", customerDto.email);
                dbConnection.com.Parameters.AddWithValue("@phone_number", customerDto.phone_number);                

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

                dbConnection.com.ExecuteNonQuery();
                dbConnection.com.Dispose();
                dbConnection.conn.Close();


            // catches test data from webhook in case it's updated or newly created needs to be implemented

            return 200;
        }

        // POST api/<controller>/<source>
        // customers from database get sent to webshop via POST request
        [HttpPost]
        [Route("api/v1/customers/database")]
        public string PostCustomerToWebshop()
        {
            // declare variables
            String SQLString = "SELECT * FROM customers";
            DataTable dataTableCustomers = new DataTable();
            DBconnection dbConnection = new DBconnection();
            CustomerDto customer = new CustomerDto();
            WebReq request;
            int numberOfCustomers = 0;

            // open connection to database
            dbConnection.openConnection();

            // create data adapter and fill dataTableCustomers with data from data adapter (customer data) 
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(SQLString, dbConnection.conn);
            dataAdapter.Fill(dataTableCustomers);
            

            for (int i = 0; i < dataTableCustomers.Rows.Count; i++)
            {
                
                //customer.id = (int)dataTableCustomers.Rows[i]["wp_user_id"];
                customer.username = dataTableCustomers.Rows[i]["username"].ToString();
                customer.first_name = dataTableCustomers.Rows[i]["first_name"].ToString();
                customer.last_name = dataTableCustomers.Rows[i]["last_name"].ToString();
                customer.email = dataTableCustomers.Rows[i]["email"].ToString();
                customer.phone_number = dataTableCustomers.Rows[i]["phone_number"].ToString();
                customer.password = dataTableCustomers.Rows[i]["password"].ToString();
                customer.role = dataTableCustomers.Rows[i]["role"].ToString();
                
                try
                {
                    customer.billing["first_name"] = dataTableCustomers.Rows[i]["first_name_billing"].ToString();
                    customer.billing["last_name"] = dataTableCustomers.Rows[i]["last_name_billing"].ToString();
                    customer.billing["company"] = dataTableCustomers.Rows[i]["company_billing"].ToString();
                    customer.billing["address"] = dataTableCustomers.Rows[i]["address_billing"].ToString();
                    customer.billing["city"] = dataTableCustomers.Rows[i]["city_billing"].ToString();
                    customer.billing["post_code"] = dataTableCustomers.Rows[i]["post_code_billing"].ToString();
                    customer.billing["country"] = dataTableCustomers.Rows[i]["country_billing"].ToString();
                    customer.billing["email"] = dataTableCustomers.Rows[i]["email_billing"].ToString();
                }
                catch
                {
                    customer.billing = null;
                }

                try
                {
                    customer.shipping["first_name"] = dataTableCustomers.Rows[i]["first_name_shipping"].ToString();
                    customer.shipping["last_name"] = dataTableCustomers.Rows[i]["last_name_shipping"].ToString();
                    customer.shipping["company"] = dataTableCustomers.Rows[i]["company_shipping"].ToString();
                    customer.shipping["address"] = dataTableCustomers.Rows[i]["address_shipping"].ToString();
                    customer.shipping["city"] = dataTableCustomers.Rows[i]["city_shipping"].ToString();
                    customer.shipping["post_code"] = dataTableCustomers.Rows[i]["post_code_shipping"].ToString();
                    customer.shipping["country"] = dataTableCustomers.Rows[i]["country_shipping"].ToString();
                }               
                catch
                {    
                    customer.shipping = null;                    
                }
                request = new WebReq();
                request.createPostRequest(customer, "customers");
                numberOfCustomers += i;         
            }

            numberOfCustomers += 1;
            dbConnection.conn.Close();
            return numberOfCustomers + " customers were added to the webshop";
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}