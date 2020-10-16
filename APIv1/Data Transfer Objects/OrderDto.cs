using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIv1.Data_Transfer_Objects
{
    public class OrderDto
    {
        public string url;

        [JsonProperty("id")]
        public int? wp_order_id { get; set; }    
        

        [JsonProperty("customer_id")]
        public int? wp_customer_id { get; set; }

        [JsonProperty("total")]
        public double total { get; set; }

        [JsonProperty("line_items")]
        public JArray line_items { get; set; }

        [JsonProperty("date_created")]
        public DateTime date { get; set; }

        public OrderDto()
        {

        }

        public OrderDto(JObject json)
        {
            wp_order_id = (int?)json["id"];
            wp_customer_id = (int)json["customer_id"];
            total= (double)json["total"];
            date = (DateTime)json["date_created"];
            line_items = json["line_items"].ToObject<JArray>();            
        }

    }
}
