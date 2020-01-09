using APIv1.Data_Transfer_Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;

namespace APIv1.Controllers
{
    public class WebshopCustomersController : ApiController
    {
        [HttpGet]
        [Route("api/v1/webshopcustomers")]
        public List<CustomerDto> GetWebshopCustomers()
        {
            List<CustomerDto> webshopCustomers = new List<CustomerDto>();

            JObject obj = null;
            CustomerDto customerDto;
            WebReq request = new WebReq();
            JsonTextReader json = request.createGetRequest("customers");            


            //loop through objects to create a CustomerDto for each 
            while (json.Read())
            {
                if (json.TokenType == JsonToken.StartObject)
                {
                    obj = JObject.Load(json);
                    customerDto = new CustomerDto(obj);
                    customerDto.url = Authentication.uriRoot + "customers/" + customerDto.wp_user_id;
                    webshopCustomers.Add(customerDto);
                }
            }
            return webshopCustomers;
        }

        //POST api/v1/<controller>/
        //POSTS customers to webshop
        [HttpPost]
        [Route("api/v1/webshopcustomers")]
        public List<CustomerDto> PostCustomerToWebshop([FromBody] JArray content)
        {
            List<CustomerDto> customersAddedToWebshop = new List<CustomerDto>();
            WebReq request;
            CustomerDto test = new CustomerDto();
            foreach (JObject customer in content)
            {
                CustomerDto customerDto = new CustomerDto(customer);
                //empty id because you cannot send an id to wordpress
                customerDto.wp_user_id = null;
                customersAddedToWebshop.Add(customerDto);

                request = new WebReq();
                request.createPostRequest(customerDto, "customers");
            }

            return customersAddedToWebshop;
        }


    }

        
}
