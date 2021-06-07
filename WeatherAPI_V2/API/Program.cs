using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using WeatherV1.WeatherAPI;

namespace WeatherV1
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Schema schema = new Schema();

            DataBase<Schema> db = new DataBase<Schema>(schema, "Weather",
                "Data Source=LAPTOP-MA7LJOFS;Initial Catalog=WeatherDB;Integrated Security=True;Pooling=False");
            //_ = db.DbCreateTable();
            IEnumerable<Schema> data = await db.DbGetData();
            foreach (Schema s in data)
            {
                Console.WriteLine(s.ID);
            }
            
            Console.ReadLine();
        }
    }
}
