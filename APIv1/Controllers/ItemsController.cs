using APIv1.Data_Transfer_Objects;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIv1.Controllers
{
    public class ItemsController : ApiController
    {
        //POST api/v1/<controller>
        //gets items from POST request and writes them into database
        [HttpPost]
        [Route("api/v1/items")]
        public List<ItemDto> PostItems([FromBody] JArray content)
        {

            int numberOfItems = 0;
            List<int> itemsAlreadyInDatabase = new List<int>();

            List<ItemDto> itemsCreated = new List<ItemDto>();


            //open DBconnection
            DBconnection dbConnection = new DBconnection();
            dbConnection.openConnection();

            foreach (JObject item in content)
            {
                ItemDto itemDto = new ItemDto(item);
                //creates DB entry for each Object                   
                dbConnection.SQLString = "INSERT INTO items (wp_item_id, item_price, item_name)" +
                    "VALUES(@wp_item_id, @item_price, @item_name);"; 
                    
                dbConnection.com = new MySqlCommand(dbConnection.SQLString, dbConnection.conn);                
                
                dbConnection.com.Parameters.AddWithValue("@wp_item_id", itemDto.wp_item_id);
                dbConnection.com.Parameters.AddWithValue("@item_price", itemDto.price);
                dbConnection.com.Parameters.AddWithValue("@item_name", itemDto.name);               
                

                //dbConnection.com.Dispose();

                //try
                //{
                    numberOfItems += dbConnection.com.ExecuteNonQuery();
                    itemDto.url = @"http://localhost:51074/api/v1/customers/" + itemDto.wp_item_id;
                    itemsCreated.Add(itemDto);
                //}
                //catch
                //{
                //    continue;
                //}

            }
            for (int i = 0; i < itemsAlreadyInDatabase.Count; i++)
            {
                content.RemoveAt(itemsAlreadyInDatabase[i]);
            }

            dbConnection.conn.Close();
            return itemsCreated;
        }
    }
}
