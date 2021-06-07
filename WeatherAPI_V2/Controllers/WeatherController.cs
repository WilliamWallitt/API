using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WeatherV1;


namespace WeatherAPI_V2.Controllers
{
    public class WeatherController : ApiController
    {
        private DataBase<Schema> _dataBase = new DataBase<Schema>(new Schema(), "Weather",
            "Data Source=LAPTOP-MA7LJOFS;Initial Catalog=WeatherDB;Integrated Security=True;Pooling=False");
        public async Task<List<Schema>> Get()

        {

            IEnumerable<Schema> weatherData = await _dataBase.DbGetData();
            return (List<Schema>)weatherData;

        }

        public async Task<List<Schema>> Get(string separator)
        {

            Debug.WriteLine(separator);

            IEnumerable<KeyValuePair<string, string>> requestParams = Request.GetQueryNameValuePairs();
            if (requestParams != null)
            {
                Dictionary<string, object> conditions = new Dictionary<string, object>();

                foreach (KeyValuePair<string, string> param in requestParams)
                {
                    if (param.Key != "separator")
                    {
                        conditions.Add(param.Key, param.Value);
                    }
                }
                IEnumerable<Schema> weatherData = await _dataBase.DbGetData(conditions, separator);
                return (List<Schema>)weatherData;
            }
            else
            {
                IEnumerable<Schema> weatherData = await _dataBase.DbGetData(separator:separator);
                return (List<Schema>)weatherData;
            }

        }
    }
}
