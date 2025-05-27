using System;
using Xunit;
using OpenSchema; // Assuming OpenDB is in this namespace

namespace OpenSchema.Tests
{
    public class OpenDbTests
    {
        [Fact]
        public void GetService_ReturnsLiteDbService_ForLiteDbProvider()
        {
            // Arrange
            string provider = "litedb";
            string connection = "test.db";

            // Act
            var service = OpenDb.GetService(provider, connection);

            // Assert
            Assert.NotNull(service);
            Assert.IsType<LiteDbService>(service);
        }

        [Fact]
        public void GetService_ReturnsMongoDbService_ForMongoDbProvider()
        {
            // Arrange
            string provider = "mongodb";
            string connection = "mongodb://localhost:27017";
            string dbName = "testdb";

            // Act
            var service = OpenDb.GetService(provider, connection, dbName);

            // Assert
            Assert.NotNull(service);
            Assert.IsType<MongoDbService>(service);
        }

        [Fact]
        public void GetService_ReturnsRedisDbService_ForRedisDbProvider()
        {
            // Arrange
            string provider = "redisdb";
            string connection = "localhost";

            // Act
            var service = OpenDb.GetService(provider, connection);

            // Assert
            Assert.NotNull(service);
            Assert.IsType<RedisDbService>(service);
        }

        [Fact]
        public void GetService_ReturnsPostgreDbService_ForPostgreDbProvider()
        {
            // Arrange
            string provider = "postgresqldb";
            string connection = "Host=localhost;Port=5432;Username=postgres;Password=mysecretpassword;Database=testdb";

            // Act
            var service = OpenDb.GetService(provider, connection);

            // Assert
            Assert.NotNull(service);
            Assert.IsType<PostgreDbService>(service);
        }

        [Fact]
        public void GetService_ReturnsSqliteDbService_ForSqliteDbProvider()
        {
            // Arrange
            string provider = "sqlitedb";
            string connection = "test.db";

            // Act
            var service = OpenDb.GetService(provider, connection);

            // Assert
            Assert.NotNull(service);
            Assert.IsType<SqliteDbService>(service);
        }

        
    }
}
