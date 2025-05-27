using System;
using System.Collections.Generic;
using Npgsql;
using Newtonsoft.Json;

namespace OpenSchema
{
    public class PostgreDbService : IGenericDbService
    {
        private readonly string _connectionString;

        public PostgreDbService(string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=mysecretpassword;Database=openschema")
        {
            _connectionString = connectionString;
        }

        private void EnsureTableExists(string tableName)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                    $@"
                        CREATE TABLE IF NOT EXISTS ""{tableName}"" (
                            ""Id"" TEXT PRIMARY KEY,
                            ""Data"" JSONB NOT NULL
                        );
                    ";
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<object> GetAll(string collectionName)
        {
            EnsureTableExists(collectionName);
            var results = new List<object>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand($"SELECT \"Data\" FROM \"{collectionName}\"", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var jsonData = reader.GetString(0);
                            results.Add(JsonConvert.DeserializeObject<object>(jsonData));
                        }
                    }
                }
            }
            return results;
        }

        public object GetByField(string collectionName, string field, string value)
        {
            EnsureTableExists(collectionName);
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand($"SELECT \"Data\" FROM \"{collectionName}\" WHERE \"Data\"->>@field = @value", connection))
                {
                    command.Parameters.AddWithValue("field", field);
                    command.Parameters.AddWithValue("value", value);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var jsonData = reader.GetString(0);
                            return JsonConvert.DeserializeObject<object>(jsonData);
                        }
                    }
                }
            }
            return null;
        }

        public bool Insert(string collectionName, object data)
        {
            EnsureTableExists(collectionName);
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var jsonData = JsonConvert.SerializeObject(data);
                string id = Guid.NewGuid().ToString();

                try
                {
                    dynamic dynamicData = data;
                    if (dynamicData.Id != null)
                    {
                        id = dynamicData.Id.ToString();
                    }
                }
                catch { /* If Id property doesn't exist, use generated GUID */ }

                using (var command = new NpgsqlCommand($"INSERT INTO \"{collectionName}\" (\"Id\", \"Data\") VALUES (@id, @data::jsonb)", connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("data", jsonData);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(string collectionName, string field, string value, object data)
        {
            EnsureTableExists(collectionName);
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var jsonData = JsonConvert.SerializeObject(data);
                using (var command = new NpgsqlCommand($"UPDATE \"{collectionName}\" SET \"Data\" = @data::jsonb WHERE \"Data\"->>@field = @value", connection))
                {
                    command.Parameters.AddWithValue("data", jsonData);
                    command.Parameters.AddWithValue("field", field);
                    command.Parameters.AddWithValue("value", value);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string collectionName, string field, string value)
        {
            EnsureTableExists(collectionName);
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand($"DELETE FROM \"{collectionName}\" WHERE \"Data\"->>@field = @value", connection))
                {
                    command.Parameters.AddWithValue("field", field);
                    command.Parameters.AddWithValue("value", value);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
