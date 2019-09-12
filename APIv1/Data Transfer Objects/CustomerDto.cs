using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIv1.Data_Transfer_Objects
{
    public class CustomerDto
    {
        public string url;

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        

        [JsonProperty("first_name")]
        public string first_name { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }

        [JsonProperty("role")]
        public string role { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }        

        [JsonProperty("phone")]
        public string phone { get; set; }        

        [JsonProperty("password")]
        public string password { get; set; }        

        [JsonProperty("billing")]
        public Dictionary<string, string> billing { get; set; }

        [JsonProperty("shipping")]
        public Dictionary<string, string> shipping { get; set; }

        public CustomerDto()
        {

        }

        public CustomerDto(JObject json)
        {
            id = (int)json["id"];
            email = (string)json["email"];
            
            first_name = (string)json["first_name"];
            last_name = (string)json["last_name"];
            role = (string)json["role"];
            username = (string)json["username"];
            address = (string)json["address"];
            
            phone = (string)json["phone"];            
            password = (string)json["password"];
            
            //billing = json["billing"].ToObject<Dictionary<string, string>>();
            //shipping = json["shipping"].ToObject<Dictionary<string, string>>();
        }

    }
}