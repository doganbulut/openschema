using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace OpenSchema
{
    public class SqliteDbService : IGenericDbService, IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly string _databasePath;

        public SqliteDbService(string databasePath = "openschema.db")
        {
            _databasePath = databasePath;
            _connection = new SqliteConnection($"Data Source={_databasePath}");
            _connection.Open(); // Open connection once and keep it open
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        private void EnsureTableExists(string tableName)
        {
            var command = _connection.CreateCommand();
            command.CommandText =
            $@"
                CREATE TABLE IF NOT EXISTS {tableName} (
                    Id TEXT PRIMARY KEY,
                    Data TEXT NOT NULL
                );
            ";
            command.ExecuteNonQuery();
        }

        public IEnumerable<object> GetAll(string collectionName)
        {
            EnsureTableExists(collectionName);
            var results = new List<object>();
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT Data FROM {collectionName}";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var jsonData = reader.GetString(0);
                    results.Add(JsonConvert.DeserializeObject<object>(jsonData));
                }
            }
            return results;
        }

        public object GetByField(string collectionName, string field, string value)
        {
            EnsureTableExists(collectionName);
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT Data FROM {collectionName} WHERE json_extract(Data, '$.{field}') = @value";
            command.Parameters.AddWithValue("@value", value);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var jsonData = reader.GetString(0);
                    return JsonConvert.DeserializeObject<object>(jsonData);
                }
            }
            return null;
        }

        public bool Insert(string collectionName, object data)
        {
            EnsureTableExists(collectionName);
            var jsonData = JsonConvert.SerializeObject(data);
            string id = Guid.NewGuid().ToString(); // Generate a unique ID
            try
            {
                dynamic dynamicData = data;
                if (dynamicData.Id != null)
                {
                    id = dynamicData.Id.ToString();
                }
            }
            catch { /* If Id property doesn't exist, use generated GUID */ }

            var command = _connection.CreateCommand();
            command.CommandText = $"INSERT INTO {collectionName} (Id, Data) VALUES (@id, @data)";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@data", jsonData);
            return command.ExecuteNonQuery() > 0;
        }

        public bool Update(string collectionName, string field, string value, object data)
        {
            EnsureTableExists(collectionName);
            var jsonData = JsonConvert.SerializeObject(data);
            var command = _connection.CreateCommand();
            command.CommandText = $"UPDATE {collectionName} SET Data = @data WHERE json_extract(Data, '$.{field}') = @value";
            command.Parameters.AddWithValue("@data", jsonData);
            command.Parameters.AddWithValue("@value", value);
            return command.ExecuteNonQuery() > 0;
        }

        public bool Delete(string collectionName, string field, string value)
        {
            EnsureTableExists(collectionName);
            var command = _connection.CreateCommand();
            command.CommandText = $"DELETE FROM {collectionName} WHERE json_extract(Data, '$.{field}') = @value";
            command.Parameters.AddWithValue("@value", value);
            return command.ExecuteNonQuery() > 0;
        }
    }
}
