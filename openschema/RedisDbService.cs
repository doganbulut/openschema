using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace OpenSchema
{
    public class RedisDbService : IGenericDbService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisDbService(string connectionString = "localhost")
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public IEnumerable<object> GetAll(string collectionName)
        {
            // In Redis, there's no direct concept of "collections" like in document databases.
            // We'll simulate it by iterating through keys that start with a specific prefix.
            // NOTE: KEYS command can be slow on large databases. For production, consider SCAN.
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{collectionName}:*").ToList();

            var results = new List<object>();
            foreach (var key in keys)
            {
                var jsonData = _db.StringGet(key);
                if (!jsonData.IsNullOrEmpty)
                {
                    results.Add(JsonConvert.DeserializeObject<object>(jsonData));
                }
            }
            return results;
        }

        public object GetByField(string collectionName, string field, string value)
        {
            // This implementation iterates through all items in the "collection" and filters in memory.
            // For large datasets, this can be inefficient. Consider using Redis Hashes or secondary indexing
            // if querying by arbitrary fields is a frequent and performance-critical operation.
            foreach (var item in GetAll(collectionName))
            {
                var json = JsonConvert.SerializeObject(item);
                var jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                if (jObject.TryGetValue(field, out var fieldValue) && fieldValue.ToString() == value)
                {
                    return item;
                }
            }
            return null;
        }

        public bool Insert(string collectionName, object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            string id = Guid.NewGuid().ToString(); // Generate a unique ID

            // Try to extract an 'Id' property from the object if it exists
            try
            {
                dynamic dynamicData = data;
                if (dynamicData.Id != null)
                {
                    id = dynamicData.Id.ToString();
                }
            }
            catch { /* If Id property doesn't exist, use generated GUID */ }

            var key = $"{collectionName}:{id}";
            return _db.StringSet(key, jsonData);
        }

        public bool Update(string collectionName, string field, string value, object data)
        {
            // Similar to GetByField, this iterates and updates.
            // More efficient ways exist for specific Redis data structures (e.g., Hashes).
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{collectionName}:*").ToList();

            foreach (var key in keys)
            {
                var jsonData = _db.StringGet(key);
                if (!jsonData.IsNullOrEmpty)
                {
                    var item = JsonConvert.DeserializeObject<object>(jsonData);
                    var jObject = Newtonsoft.Json.Linq.JObject.Parse(jsonData);

                    if (jObject.TryGetValue(field, out var fieldValue) && fieldValue.ToString() == value)
                    {
                        // Found the item to update, now replace its data
                        var newJsonData = JsonConvert.SerializeObject(data);
                        return _db.StringSet(key, newJsonData);
                    }
                }
            }
            return false; // Item not found or updated
        }

        public bool Delete(string collectionName, string field, string value)
        {
            // Similar to GetByField, this iterates and deletes.
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{collectionName}:*").ToList();

            foreach (var key in keys)
            {
                var jsonData = _db.StringGet(key);
                if (!jsonData.IsNullOrEmpty)
                {
                    var jObject = Newtonsoft.Json.Linq.JObject.Parse(jsonData);
                    if (jObject.TryGetValue(field, out var fieldValue) && fieldValue.ToString() == value)
                    {
                        // Found the item to delete
                        return _db.KeyDelete(key);
                    }
                }
            }
            return false; // Item not found or deleted
        }
    }
}
