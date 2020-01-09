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
        public int? wp_user_id { get; set; }

        [JsonProperty("customer_id")]
        public string customer_id { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("first_name")]
        public string first_name { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }        

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("role")]
        public string role { get; set; }

        [JsonProperty("billing")]
        public Dictionary<string, string> billing { get; set; }

        [JsonProperty("shipping")]
        public Dictionary<string, string> shipping { get; set; }

        public CustomerDto()
        {

        }

        public CustomerDto(JObject json)
        {
            wp_user_id = (int?)json["id"];
            username = (string)json["username"];
            first_name = (string)json["first_name"];
            last_name = (string)json["last_name"];
            address = (string)json["address"];
            email = (string)json["email"];
            phone = (string)json["phone"];            
            //password = (string)json["password"];
            role = (string)json["role"];

            try
            {
                billing = json["billing"].ToObject<Dictionary<string, string>>();
            }
            catch 
            {

                billing = null;
            }
            try
            {
                shipping = json["shipping"].ToObject<Dictionary<string, string>>();
            }
            catch
            {

                shipping = null;
            }
        }

    }
}