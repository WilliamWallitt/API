using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace WeatherV1
{
    class DataBase<T> : CrudQueries
    {
        private readonly SqlConnection _db;
        private T _schemaClass;
        public DataBase(T apiSchema, string tableName, string connectionPath) : base(apiSchema, tableName)
        {
            _db = new SqlConnection(connectionPath);
            _schemaClass = apiSchema;
            Console.WriteLine("Connection to -> " + connectionPath + ": Established");
        }

        public async Task DbCreateTable()
        {
            try
            {
                Console.WriteLine(CreateDbQuery());
                await _db.ExecuteAsync(CreateDbQuery());
                Console.WriteLine("Table created");
                
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DbDropTable()
        {
            try
            {
                await _db.ExecuteAsync(DropDbQuery());
                Console.WriteLine("Table dropped");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DbInsertData(Dictionary<string, object> data = null)
        {
            try
            {
                await _db.ExecuteAsync(CreateInsertQuery(data));
                Console.WriteLine("Row inserted");
            } catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<IEnumerable<T>> DbGetData(Dictionary<string, object> conditions = null, string separator="AND")
        {
            try
            {
                return await _db.QueryAsync<T>(CreateGetQuery(conditions, separator));
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public async Task DbUpdateData(Dictionary<string, object> values, Dictionary<string, object> conditions)
        {
            try
            {
                Console.WriteLine(CreateUpdateQuery(values, conditions));
                await _db.ExecuteAsync(CreateUpdateQuery(values, conditions));
                Console.WriteLine("Row updated");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DbDeleteData(Dictionary<string, object> conditions = null, string separator="AND")
        {
            try
            {
                await _db.ExecuteAsync(CreateDeleteQuery(conditions, separator));
                Console.WriteLine("Rows deleted");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
        }

    }

}
