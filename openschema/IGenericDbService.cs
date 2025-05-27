using System.Collections.Generic;

public interface IGenericDbService
{
    public IEnumerable<object> GetAll(string collectionName);
    object GetByField(string collectionName, string field, string value);
    public bool Insert(string collectionName, object data);
    public bool Update(string collectionName, string field, string value, object data);
    public bool Delete(string collectionName, string field, string value);
}