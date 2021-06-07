using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherV1.WeatherAPI;

namespace WeatherV1
{
    public class Schema
    {
        public int ID { get; set; }
        public double temperature { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public string description { get; set; }
        public string main { get; set; }
        public double speed { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }


        public async Task PopulateSchema()
        {
            HttpService<WeatherJSON> service = new HttpService<WeatherJSON>();
            service.CreateQuery("lat=43.7102", "lon=7.2620", "units=metric", "appid=a70d1cd8dca6f38d63697c5ac585c18e");
            WeatherJSON weatherJsonClass = await service.Json();
            if (weatherJsonClass == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                temperature = weatherJsonClass.main.temp;
                feels_like = weatherJsonClass.main.feels_like;
                temp_min = weatherJsonClass.main.temp;
                temp_max = weatherJsonClass.main.temp_max;
                pressure = weatherJsonClass.main.pressure;
                humidity = weatherJsonClass.main.humidity;
                description = weatherJsonClass.weather[0].description;
                main = description = weatherJsonClass.weather[0].main;
                speed = weatherJsonClass.wind.speed;
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                sunrise = dateTime.AddSeconds(Convert.ToDouble(weatherJsonClass.sys.sunrise)).ToLocalTime().ToString();
                sunset = dateTime.AddSeconds(Convert.ToDouble(weatherJsonClass.sys.sunset)).ToLocalTime().ToString();

            }

        }
    }
}

