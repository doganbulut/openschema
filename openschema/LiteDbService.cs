using System;
using System.Collections.Generic;
using LiteDB;

public class LiteDbService : IGenericDbService, IDisposable
{
    private readonly LiteDatabase _db;

    public LiteDbService(string dbPath)
    {
        _db = new LiteDatabase(dbPath);
    }

    public void Dispose()
    {
        _db.Dispose();
    }

    public IEnumerable<object> GetAll(string collectionName)
    {
        var collection = _db.GetCollection(collectionName);
        return collection.FindAll();
    }

    public object GetByField(string collectionName, string field, string value)
    {
        var collection = _db.GetCollection(collectionName);
        if (field.Equals("Id", StringComparison.OrdinalIgnoreCase))
        {
            // If the field is "Id", assume it's the primary key and use FindById
            return collection.FindById(new BsonValue(value));
        }
        return collection.FindOne($"$.{field} = '{value}'");
    }

    public bool Insert(string collectionName, object data)
    {
        var collection = _db.GetCollection(collectionName);
        return collection.Insert(BsonMapper.Global.ToDocument(data)) != null;
    }

    public bool Update(string collectionName, string field, string value, object data)
    {
        var collection = _db.GetCollection(collectionName);
        BsonDocument existing = null;

        if (field.Equals("Id", StringComparison.OrdinalIgnoreCase))
        {
            existing = collection.FindById(new BsonValue(value));
        }
        else
        {
            existing = collection.FindOne($"$.{field} = '{value}'");
        }

        if (existing != null && existing.TryGetValue("_id", out var id))
        {
            return collection.Update(id, BsonMapper.Global.ToDocument(data));
        }
        return false;
    }

    public bool Delete(string collectionName, string field, string value)
    {
        var collection = _db.GetCollection(collectionName);
        if (field.Equals("Id", StringComparison.OrdinalIgnoreCase))
        {
            // If the field is "Id", assume it's the primary key and use Delete(BsonValue id)
            return collection.Delete(new BsonValue(value));
        }
        var existing = collection.FindOne($"$.{field} = '{value}'");
        if (existing != null && existing.TryGetValue("_id", out var id))
        {
            return collection.Delete(id);
        }
        return false;
    }
}
