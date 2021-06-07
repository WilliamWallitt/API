using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft;
using Newtonsoft.Json;


namespace WeatherV1.WeatherAPI
{
    class HttpService<T>
    {
        private string _req;
        private readonly WebClient _client;
        public HttpService(string API_req = "https://api.openweathermap.org/data/2.5/weather?")
        {
            _req = API_req;
            _client = new WebClient();
        }

        public async Task<string> Request()
        {
            string result = await _client.DownloadStringTaskAsync(_req);
            _client.Dispose();
            return result;
        }

        public async Task<T> Json()
        {
            // deserialize string req into class, then we can get the info out of the WeatherJSON class
            T jsonData = JsonConvert.DeserializeObject<T>(await Request());
            return jsonData;
        }


        public void CreateQuery(params string[] args)
        {
            string queryParams = "";
            foreach (string arg in args)
            {
                queryParams += arg + "&";
            }
            queryParams = queryParams.Remove(queryParams.Length - 1);
            _req += queryParams;
        }

    }
}
