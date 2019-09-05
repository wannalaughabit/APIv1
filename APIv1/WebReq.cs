using APIv1.Data_Transfer_Objects;
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

            // turn off validation of certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };            
        
            response = (HttpWebResponse)request.GetResponse();
            stream = response.GetResponseStream();
            reader = new StreamReader(stream);

            // add JsonTextReader to get JSON objects (customers) from StreamReader
            JsonTextReader json = new JsonTextReader(reader);

            return json;
        }

        public int createPostRequest(CustomerDto customerDto, string endpoint)
        {
            string html = string.Empty;
            string uri = Authentication.uriRoot + endpoint;

            // Zertifikatvalidierung abschalten
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("Authorization", "Basic " + Authentication.encoded);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(customerDto);
                

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

            }
            return 200;
        }
    }
}