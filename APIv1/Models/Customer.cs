using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIv1.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Dictionary<string, string> Billing { get; set; }
        public Dictionary<string, string> Shipping { get; set; }
    }
}
