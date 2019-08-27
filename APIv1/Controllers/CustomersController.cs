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
        public string GetCustomers()
        {
            // variables for web request and looping through JSON Objects
            string html = string.Empty;
            string uri = Authentication.uriRoot + @"customers/";
            string jsonResultUser = "";
            int numberOfCustomers = 0;

            JObject obj = null;
            CustomerDto customerDto = new CustomerDto();

            //variables for DB connection
            String SQLString;
            MySqlCommand com;

            //open DB connection
            String connectionString = @"Server=localhost; Database=webshop; User ID=API; Allow User Variables=True";
            DBconnection.conn = new MySqlConnection(connectionString);

            DBconnection.conn.Open();

            //generate web request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("Authorization", "Basic " + Authentication.encoded);


            request.Method = "GET";
            request.ContentType = "application/json";

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            // turn off validation of certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            // add JsonTextReader to get single JSON objects (customer) from StreamReader
            JsonTextReader json = new JsonTextReader(reader);


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
                    SQLString = "INSERT INTO customers (wp_user_id, username, first_name, last_name, email, phone_number, password, " +
                        "first_name_billing, last_name_billing, company_billing, address_billing, city_billing, post_code_billing, country_billing, email_billing," +
                        "first_name_shipping, last_name_shipping, company_shipping, address_shipping, city_shipping, post_code_shipping, country_shipping)" +
                        "VALUES(@wp_user_id, @username, @first_name, @last_name, @email, @phone_number, @password, " +
                        "@first_name_billing, @last_name_billing, @company_billing, @address_billing, @city_billing, @post_code_billing, @country_billing, @email_billing," +
                        "@first_name_shipping, @last_name_shipping, @company_shipping, @address_shipping, @city_shipping, @post_code_shipping, @country_shipping)";
                    com = new MySqlCommand(SQLString, DBconnection.conn);                    
                    
                    com.Parameters.AddWithValue("@wp_user_id", customerDto.id);
                    com.Parameters.AddWithValue("@username", customerDto.username);
                    com.Parameters.AddWithValue("@first_name", customerDto.first_name);
                    com.Parameters.AddWithValue("@last_name", customerDto.last_name);
                    com.Parameters.AddWithValue("@email", customerDto.Email);
                    com.Parameters.AddWithValue("@phone_number", customerDto.Email);
                    com.Parameters.AddWithValue("@password", customerDto.Email);

                    com.Parameters.AddWithValue("@first_name_billing", customerDto.billing["first_name"]);
                    com.Parameters.AddWithValue("@last_name_billing", customerDto.billing["last_name"]);
                    com.Parameters.AddWithValue("@company_billing", customerDto.billing["company"]);
                    com.Parameters.AddWithValue("@address_billing", customerDto.billing["address_1"]);
                    com.Parameters.AddWithValue("@city_billing", customerDto.billing["city"]);
                    com.Parameters.AddWithValue("@post_code_billing", customerDto.billing["postcode"]);
                    com.Parameters.AddWithValue("@country_billing", customerDto.billing["country"]);
                    com.Parameters.AddWithValue("@email_billing", customerDto.billing["email"]);

                    com.Parameters.AddWithValue("@first_name_shipping", customerDto.shipping["first_name"]);
                    com.Parameters.AddWithValue("@last_name_shipping", customerDto.shipping["last_name"]);
                    com.Parameters.AddWithValue("@company_shipping", customerDto.shipping["company"]);
                    com.Parameters.AddWithValue("@address_shipping", customerDto.shipping["address_1"]);
                    com.Parameters.AddWithValue("@city_shipping", customerDto.shipping["city"]);
                    com.Parameters.AddWithValue("@post_code_shipping", customerDto.shipping["postcode"]);
                    com.Parameters.AddWithValue("@country_shipping", customerDto.shipping["country"]);
                    
                    com.Dispose();
                    numberOfCustomers += com.ExecuteNonQuery();
                    

                }                
            }

            DBconnection.conn.Close();
            stream.Close();
            response.Close();

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