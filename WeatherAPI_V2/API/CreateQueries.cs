using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherV1
{
    class CrudQueries : DatabaseDataTypes
    {
        private readonly Dictionary<string, object> _internalSchemaAttributes;
        private readonly string _internalSchemaTable;
        public object Schema { get; set;}
        public CrudQueries(object apiSchema, string tableName)
        {
            Schema = apiSchema;
            _internalSchemaAttributes = new Dictionary<string, object>();
            _internalSchemaTable = tableName;
            foreach (PropertyInfo attribute in apiSchema.GetType().GetProperties())
            {
                if (attribute.Name is "Id" || attribute.Name == "id" || attribute.Name == "ID")
                {
                    continue;
                }
                object propertyValue = attribute.GetValue(apiSchema, new object[] { });
                string propertyName = attribute.Name.ToLower();
                _internalSchemaAttributes.Add(propertyName, propertyValue);
            }
        }

        protected string CreateDbQuery()
        {
            string query = "CREATE TABLE [dbo].[" + _internalSchemaTable +
                           "] ( [ID] INT IDENTITY (1, 1) NOT NULL, ";
            foreach (KeyValuePair<string, object> attribute in _internalSchemaAttributes)
            {
                query += GetType(attribute.Value, attribute.Key);
            }

            return query + "PRIMARY KEY CLUSTERED ([ID] ASC));";
        }

        protected string DropDbQuery()
        {
            return "DROP TABLE " + _internalSchemaTable;
        }

        protected string CreateInsertQuery(Dictionary<string, object> data = null)
        {
            
            string values;
            var attributes = values = string.Empty;

            if (data == null)
            {
                foreach (KeyValuePair<string, object> attribute in _internalSchemaAttributes)
                {
                    attributes += attribute.Key + ",";
                    values += "'" + attribute.Value.ToString() + "',";
                }
            }
            else
            {
                foreach (KeyValuePair<string, object> d in data)
                {
                    attributes += d.Key + ",";
                    values += "'" + d.Value.ToString() + "',";
                }
            }

            return "INSERT INTO " + _internalSchemaTable + 
                   "(" + attributes.Substring(0, attributes.Length - 1) + ")" 
                   + " VALUES " + 
                   "(" + values.Substring(0, values.Length - 1) + ")";
            
        }

        protected string CreateDeleteQuery(IEnumerable<Tuple<string, object>> conditions, string separator = "AND")
        {
            string query;

            if (conditions == null)
            {
                query = "DELETE * FROM " + _internalSchemaTable;
                return query;
            }

            query = "DELETE FROM " + _internalSchemaTable + " WHERE ";

            foreach (Tuple<string, object> condition in conditions)
            {
                query += condition.Item1 + " LIKE '%" + condition.Item2.ToString() + "%' " + separator + " ";
            }

            return query.Substring(0, query.Length - (separator.Length + 2));

        }

        protected string CreateGetQuery(IEnumerable<Tuple<string, object>> conditions, string separator="AND")
        {
            string query;
            if (conditions == null)
            {
                query = "SELECT * FROM " + _internalSchemaTable;
                return query;
            }
            else
            {
                query = "SELECT * FROM " + _internalSchemaTable + " WHERE ";
                foreach (Tuple<string, object> condition in conditions)
                {
                    query += condition.Item1 + " LIKE '%" + condition.Item2.ToString() + "%' " + separator + " ";
                }
                Debug.WriteLine(query.Substring(0, query.Length - (separator.Length + 2)));
                return query.Substring(0, query.Length - (separator.Length + 2));
            }
            
        }

        protected string CreateUpdateQuery(Dictionary<string, object> values, Dictionary<string, object> conditions)
        {
            if (conditions == null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            string query = "UPDATE " + _internalSchemaTable + " SET ";
            foreach (KeyValuePair<string, object> value in values)
            {
                query += value.Key + "='" + value.Value.ToString() + "', ";
            }
            query = query.Substring(0, query.Length - 2) + " WHERE ";

            foreach (KeyValuePair<string, object> condition in conditions)
            {
                query += condition.Key + "='" + condition.Value.ToString() + "', AND";
            }

            return query.Substring(0, query.Length - 5);
        }

    }
}
