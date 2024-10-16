using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Policy;

namespace Itventory.web
{
    public static class APIConnection
    {
        private static string url = "https://localhost:7170/";

        public static string Url { get { return url; } }

        public static string GetRequest(string urlComplement)
        {
            
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + urlComplement;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                string content = response.Content.ReadAsStringAsync().Result;

                return content;
            }
        }

    }
}
