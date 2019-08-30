using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace APIv1
{
    public class WebReq
    {
        //declaring variables for Get Request
        public Stream stream;
        public StreamReader reader;
        public HttpWebResponse response;


        public JsonTextReader createGetRequest(string endpoint)
        {            
            //generate web request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Authentication.uriRoot + endpoint);
            request.Headers.Add("Authorization", "Basic " + Authentication.encoded);


            request.Method = "GET";
            request.ContentType = "application/json";

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            // turn off validation of certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            response = (HttpWebResponse)request.GetResponse();
            stream = response.GetResponseStream();
            reader = new StreamReader(stream);

            // add JsonTextReader to get single JSON objects (customer) from StreamReader
            JsonTextReader json = new JsonTextReader(reader);

            return json;
        }
    }
}