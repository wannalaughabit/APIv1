using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIv1.Data_Transfer_Objects
{
    public class ItemDto
    {
        public string url;

        [JsonProperty("id")]
        public int? wp_item_id { get; set; }

        [JsonProperty("sku")]
        public string item_id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("price")]
        public int price { get; set; }

        

        public ItemDto()
        {

        }

        public ItemDto(JObject json)
        {
            wp_item_id = (int?)json["id"];
            item_id = (string)json["sku"];
            name = (string)json["name"];
            price = (int)json["price"];            
        }

    }
}
