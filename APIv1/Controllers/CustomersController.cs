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
        [HttpGet]
        [Route ("api/v1/customers/webshop")]
        public string GetCustomers()
        {
            //open DBconnection
            DBconnection dbConnection = new DBconnection();
            dbConnection.openConnection();

            // variables for web request and looping through JSON Objects
                        
            string jsonResultUser = "";
            int numberOfCustomers = 0;

            JObject obj = null;
            CustomerDto customerDto = new CustomerDto();
            WebReq request = new WebReq();
            JsonTextReader json = request.createGetRequest("customers");

            //loop through objects to create a CustomerDto for each and pass it to CreateCustomer
            while (json.Read())
            {
                if (json.TokenType == JsonToken.StartObject)
                {
                    obj = JObject.Load(json);
                    customerDto = new CustomerDto(obj);
                    jsonResultUser = JsonConvert.SerializeObject(customerDto);                    
                    File.AppendAllText("D:\\Test\\customers.txt", jsonResultUser);

                    //creates DB entry for each Object                    
                    
                    dbConnection.SQLString = "INSERT INTO customers (wp_user_id, username, first_name, last_name, email, phone_number, password, " +
                        "first_name_billing, last_name_billing, company_billing, address_billing, city_billing, post_code_billing, country_billing, email_billing," +
                        "first_name_shipping, last_name_shipping, company_shipping, address_shipping, city_shipping, post_code_shipping, country_shipping)" +
                        "VALUES(@wp_user_id, @username, @first_name, @last_name, @email, @phone_number, @password, " +
                        "@first_name_billing, @last_name_billing, @company_billing, @address_billing, @city_billing, @post_code_billing, @country_billing, @email_billing," +
                        "@first_name_shipping, @last_name_shipping, @company_shipping, @address_shipping, @city_shipping, @post_code_shipping, @country_shipping)";
                    dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);                    
                    
                    dbConnection.com.Parameters.AddWithValue("@wp_user_id", customerDto.id);
                    dbConnection.com.Parameters.AddWithValue("@username", customerDto.username);
                    dbConnection.com.Parameters.AddWithValue("@first_name", customerDto.first_name);
                    dbConnection.com.Parameters.AddWithValue("@last_name", customerDto.last_name);
                    dbConnection.com.Parameters.AddWithValue("@email", customerDto.Email);
                    dbConnection.com.Parameters.AddWithValue("@phone_number", customerDto.Email);
                    dbConnection.com.Parameters.AddWithValue("@password", customerDto.Email);

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
                    
                    numberOfCustomers += dbConnection.com.ExecuteNonQuery();
                }                
            }

            dbConnection.conn.Close();
            request.stream.Close();
            request.response.Close();
            string returnMessage = numberOfCustomers + " customers were added to the database.";
            return returnMessage;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public int Post([FromBody]string value)
        {
            return 200;
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