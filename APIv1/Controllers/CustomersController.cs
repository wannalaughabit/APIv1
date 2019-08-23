using APIv1.Data_Transfer_Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            string html = string.Empty;
            string uri = Authentication.uriRoot + @"customers/";
            JObject obj;
            CustomerDto customerDto = new CustomerDto();


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
                }                
            }
            
            string jsonResult = JsonConvert.SerializeObject(customerDto);
            string result = reader.ReadToEnd();

            stream.Close();
            response.Close();
            File.WriteAllText("D:\\Test\\customers.txt", jsonResult);
            return result;
            
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
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