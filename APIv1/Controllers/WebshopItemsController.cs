using APIv1.Data_Transfer_Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIv1.Controllers
{
    public class WebshopItemsController : ApiController
    {
        [HttpGet]
        [Route("api/v1/webshopitems")]
        public List<ItemDto> GetWebshopItems()
        {
            List<ItemDto> webshopItems = new List<ItemDto>();

            JObject obj = null;
            ItemDto itemDto;
            WebReq request = new WebReq();
            JsonTextReader json = request.createGetRequest("products");


            //loop through objects to create a CustomerDto for each 
            while (json.Read())
            {
                if (json.TokenType == JsonToken.StartObject)
                {
                    obj = JObject.Load(json);
                    itemDto = new ItemDto(obj);
                    itemDto.url = Authentication.uriRoot + "products/" + itemDto.item_id;
                    webshopItems.Add(itemDto);
                }
            }
            return webshopItems;
        }
    }
}
