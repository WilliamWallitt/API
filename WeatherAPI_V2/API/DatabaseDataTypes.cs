using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeatherV1
{
    class DatabaseDataTypes
    {
        public static string GetType(object type, string name)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (name == null) throw new ArgumentNullException(nameof(name));

            if (type is int)
            {
                return "[" + name + "]" + " INT NOT NULL, ";
            }

            if (type is double)
            {
                return "[" + name + "]" + " DECIMAL (18, 3) NOT NULL, ";
            }

            return "[" + name + "]" + " VARCHAR (255) NOT NULL, ";
            
        }
    }
}
