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
            


            //loop through objects to create a CustomerDto for each and pass it to CreateCustomer
            while (json.Read())
            {
                if (json.TokenType == JsonToken.StartObject)
                {
                    obj = JObject.Load(json);
                    customerDto = new CustomerDto(obj);
                    customerDto.url = Authentication.uriRoot + "customers/" + customerDto.id;
                    webshopCustomers.Add(customerDto);
                }
            }
            return webshopCustomers;
        }

        // POST api/v1/<controller>/
        // POSTS several customers to webshop 
        
        [HttpPost]
        [Route("api/v1/webshopcustomers")]
        public List<CustomerDto> PostCustomerToWebshop(JArray customers)
        {
            List<CustomerDto> customersAddedToWebshop = new List<CustomerDto>();
            CustomerDto customerDto; 
            WebReq request = new WebReq();

            foreach (JObject customer in customers)
            {
                customerDto = new CustomerDto();
                // cannot pass customerDto to wordpress directly because wordpress cannot use arrays
                customerDto.url = Authentication.uriRoot + "customers/" + customerDto.id;
                customerDto.username = customer.Value<string>("username");
                customerDto.email = customer.Value<string>("email");
                if(customer.Value<string>("password") != null)
                {
                    customerDto.password = customer.Value<string>("password");
                }

                customerDto.first_name = customer.Value<string>("first_name");
                customerDto.last_name = customer.Value<string>("last_name");
                customerDto.address = customer.Value<string>("address");
                customerDto.phone = customer.Value<string>("phone");                
                customerDto.role = customer.Value<string>("rolse");

                try
                {
                    customerDto.billing["first_name"] = customer.Value<string>("first_name_billing");
                    customerDto.billing["last_name"] = customer.Value<string>("last_name_billing");
                    customerDto.billing["company"] = customer.Value<string>("company_billing");
                    customerDto.billing["address"] = customer.Value<string>("address_billing");
                    customerDto.billing["city"] = customer.Value<string>("city_billing");
                    customerDto.billing["post_code"] = customer.Value<string>("post_code_billing");
                    customerDto.billing["country"] = customer.Value<string>("country_billing");
                    customerDto.billing["email"] = customer.Value<string>("email_billing");
                }
                catch 
                {

                    customerDto.billing = null;
                }

                try
                {
                    customerDto.shipping["first_name"] = customer.Value<string>("first_name_shipping");
                    customerDto.shipping["last_name"] = customer.Value<string>("last_name_shipping");
                    customerDto.shipping["company"] = customer.Value<string>("company_shipping");
                    customerDto.shipping["address"] = customer.Value<string>("address_shipping");
                    customerDto.shipping["city"] = customer.Value<string>("city_shipping");
                    customerDto.shipping["post_code"] = customer.Value<string>("post_code_shipping");
                    customerDto.shipping["country"] = customer.Value<string>("country_shipping");
                    customerDto.shipping["email"] = customer.Value<string>("email_shipping");
                }
                catch 
                {

                    customerDto.shipping = null;
                }                 

                try
                {                    
                    request.createPostRequest(customerDto, "customers");
                    customersAddedToWebshop.Add(customerDto);
                }
                catch 
                {
                    continue; 
                }
            }

            return customersAddedToWebshop;
        }

        // POST api/v1/<controller>/customer
        // POSTS single customer to webshop 
        // returns List so it'll return an empty array instead of a CustomerDto when the customer is already in the database
        [HttpPost]
        [Route("api/v1/webshopcustomers/customer")]
        public List<CustomerDto> PostCustomerToWebshop(JObject customer)
        {
            List<CustomerDto> customersAddedToWebshop = new List<CustomerDto>();
            CustomerDto customerDto = new CustomerDto();
            WebReq request = new WebReq();

            // cannot pass customerDto to wordpress directly because wordpress cannot use arrays
            customerDto.url = Authentication.uriRoot + "customers/" + customerDto.id;
            customerDto.username = customer.Value<string>("username");
            customerDto.email = customer.Value<string>("email");
            if (customer.Value<string>("password") != null)
            {
                customerDto.password = customer.Value<string>("password");
            }

            customerDto.first_name = customer.Value<string>("first_name");
            customerDto.last_name = customer.Value<string>("last_name");
            customerDto.address = customer.Value<string>("address");
            customerDto.phone = customer.Value<string>("phone");
            customerDto.role = customer.Value<string>("rolse");

            try
            {
                customerDto.billing["first_name"] = customer.Value<string>("first_name_billing");
                customerDto.billing["last_name"] = customer.Value<string>("last_name_billing");
                customerDto.billing["company"] = customer.Value<string>("company_billing");
                customerDto.billing["address"] = customer.Value<string>("address_billing");
                customerDto.billing["city"] = customer.Value<string>("city_billing");
                customerDto.billing["post_code"] = customer.Value<string>("post_code_billing");
                customerDto.billing["country"] = customer.Value<string>("country_billing");
                customerDto.billing["email"] = customer.Value<string>("email_billing");
            }
            catch
            {

                customerDto.billing = null;
            }

            try
            {
                customerDto.shipping["first_name"] = customer.Value<string>("first_name_shipping");
                customerDto.shipping["last_name"] = customer.Value<string>("last_name_shipping");
                customerDto.shipping["company"] = customer.Value<string>("company_shipping");
                customerDto.shipping["address"] = customer.Value<string>("address_shipping");
                customerDto.shipping["city"] = customer.Value<string>("city_shipping");
                customerDto.shipping["post_code"] = customer.Value<string>("post_code_shipping");
                customerDto.shipping["country"] = customer.Value<string>("country_shipping");
                customerDto.shipping["email"] = customer.Value<string>("email_shipping");
            }
            catch
            {

                customerDto.shipping = null;
            }

            try
            {
                request.createPostRequest(customerDto, "customers");
                customersAddedToWebshop.Add(customerDto);
            }
            catch
            {
                
            } 

            return customersAddedToWebshop;
        }
    }

        
}
