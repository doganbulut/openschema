using System;
using OpenSchema;

namespace OpenSchema
{
    public static class OpenDb
    {
        public static IGenericDbService GetService(string provider, string connection, string dbName = null)
        {
        return provider.ToLower() switch
        {
            "litedb" => new LiteDbService(connection),
            "mongodb" => new MongoDbService(connection, dbName),
            "redisdb" => new RedisDbService(connection),
            "postgresqldb" => new PostgreDbService(connection),
            "sqlitedb" => new SqliteDbService(connection),
            _ => throw new NotSupportedException("Unknown provider"),
        };
    }
}
}
