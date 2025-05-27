using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

public class MongoDbService : IGenericDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(string connectionString, string dbName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(dbName);
    }

    public IEnumerable<object> GetAll(string collectionName)
    {
        var collection = _database.GetCollection<BsonDocument>(collectionName);
        var filter = Builders<BsonDocument>.Filter.Empty;
        return collection.Find(filter).ToList();
    }

    private FilterDefinition<BsonDocument> CreateIdFilter(string field, string value)
    {
        if (field == "Id" && ObjectId.TryParse(value, out ObjectId objectId))
        {
            return Builders<BsonDocument>.Filter.Eq("_id", objectId);
        }
        return Builders<BsonDocument>.Filter.Eq(field, value);
    }

    public object GetByField(string collectionName, string field, string value)
    {
        var collection = _database.GetCollection<BsonDocument>(collectionName);
        var filter = CreateIdFilter(field, value);
        return collection.Find(filter).FirstOrDefault();
    }

    public bool Insert(string collectionName, object data)
    {
        var collection = _database.GetCollection<BsonDocument>(collectionName);
        collection.InsertOne(BsonDocument.Parse(JsonConvert.SerializeObject(data)));
        return true;
    }

    public bool Update(string collectionName, string field, string value, object data)
    {
        var collection = _database.GetCollection<BsonDocument>(collectionName);
        var filter = CreateIdFilter(field, value);
        var result = collection.ReplaceOne(filter, BsonDocument.Parse(JsonConvert.SerializeObject(data)));
        return result.ModifiedCount > 0;
    }

    public bool Delete(string collectionName, string field, string value)
    {
        var collection = _database.GetCollection<BsonDocument>(collectionName);
        var filter = CreateIdFilter(field, value);
        var result = collection.DeleteOne(filter);
        return result.DeletedCount > 0;
    }
}
