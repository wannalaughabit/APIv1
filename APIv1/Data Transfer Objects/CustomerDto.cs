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
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("first_name")]
        public string first_name { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

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
            username = (string)json["username"];
            first_name = (string)json["first_name"];
            last_name = (string)json["last_name"];
            address = (string)json["address"];
            Email = (string)json["email"];
            password = (string)json["password"];
            billing = json["billing"].ToObject<Dictionary<string, string>>();
            shipping = json["shipping"].ToObject<Dictionary<string, string>>();
        }

    }
}