using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace APIv1
{
    public static class Authentication
    {
        public static String uriRoot = @"https://localhost/webshop/wp-json/wc/v3/";
        public static String username = File.ReadAllText(@"D:\key.txt");
        public static String password = File.ReadAllText(@"D:\secret.txt");
        public static String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
    }
}