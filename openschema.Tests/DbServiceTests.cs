using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using openschema;
using Newtonsoft.Json;
using StackExchange.Redis;
using MongoDB.Driver;
using LiteDB;
using OpenSchema;
using Npgsql;


namespace openschema.Tests
{
    public class DbServiceTests
    {
        // A simple entity for testing
        public class TestEntity
        {
            [MongoDB.Bson.Serialization.Attributes.BsonId]
            [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
            public string Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [Fact]
        public void SqliteDbService_CrudOperations_WorkCorrectly()
        {
            // Use an in-memory SQLite database for testing
            using (var service = new SqliteDbService(":memory:"))
            {
            var collectionName = "TestCollectionSqlite";

            // Test Insert
            var entity1 = new TestEntity { Id = "1", Name = "Alice", Age = 30 };
            Assert.True(service.Insert(collectionName, entity1));

            var entity2 = new TestEntity { Id = "2", Name = "Bob", Age = 25 };
            Assert.True(service.Insert(collectionName, entity2));

            // Test GetAll
            var allEntities = service.GetAll(collectionName)
                                     .Select(item => JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(item)))
                                     .ToList();
            Assert.Equal(2, allEntities.Count);
            Assert.Contains(allEntities, e => e.Id == "1" && e.Name == "Alice");
            Assert.Contains(allEntities, e => e.Id == "2" && e.Name == "Bob");

            // Test GetByField
            var retrievedEntity = JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(service.GetByField(collectionName, "Name", "Alice")));
            Assert.NotNull(retrievedEntity);
            Assert.Equal("1", retrievedEntity.Id);

            // Test Update
            entity1.Age = 31;
            Assert.True(service.Update(collectionName, "Id", "1", entity1));
            retrievedEntity = JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(service.GetByField(collectionName, "Id", "1")));
            Assert.NotNull(retrievedEntity);
            Assert.Equal(31, retrievedEntity.Age);

            // Test Delete
            Assert.True(service.Delete(collectionName, "Id", "2"));
            allEntities = service.GetAll(collectionName)
                                     .Select(item => JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(item)))
                                     .ToList();
            Assert.Single(allEntities);
            Assert.DoesNotContain(allEntities, e => e.Id == "2");
            }
        }

        [Fact]
        public void RedisDbService_CrudOperations_WorkCorrectly()
        {
            // Ensure Redis is running locally or adjust connection string
            var service = new RedisDbService("localhost:6379");
            var collectionName = "TestCollectionRedis"; // Use a distinct collection name for tests

            // Clean up previous test data if any
            var server = ConnectionMultiplexer.Connect("localhost:6379").GetServer("localhost:6379");
            foreach (var key in server.Keys(pattern: $"{collectionName}:*"))
            {
                service.Delete(collectionName, "Id", key.ToString().Split(':')[1]); // This is a hacky way to delete by ID
            }

            // Test Insert
            var entity1 = new TestEntity { Id = "3", Name = "Charlie", Age = 40 };
            Assert.True(service.Insert(collectionName, entity1));

            var entity2 = new TestEntity { Id = "4", Name = "David", Age = 35 };
            Assert.True(service.Insert(collectionName, entity2));

            // Test GetAll
            var allEntities = service.GetAll(collectionName)
                                     .Select(item => JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(item)))
                                     .ToList();
            Assert.Equal(2, allEntities.Count);
            Assert.Contains(allEntities, e => e.Id == "3" && e.Name == "Charlie");
            Assert.Contains(allEntities, e => e.Id == "4" && e.Name == "David");

            // Test GetByField (Note: This is inefficient for Redis as implemented)
            var retrievedEntity = JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(service.GetByField(collectionName, "Name", "Charlie")));
            Assert.NotNull(retrievedEntity);
            Assert.Equal("3", retrievedEntity.Id);

            // Test Update
            entity1.Age = 41;
            Assert.True(service.Update(collectionName, "Id", "3", entity1));
            retrievedEntity = JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(service.GetByField(collectionName, "Id", "3")));
            Assert.NotNull(retrievedEntity);
            Assert.Equal(41, retrievedEntity.Age);

            // Test Delete
            Assert.True(service.Delete(collectionName, "Id", "4"));
            allEntities = service.GetAll(collectionName)
                                     .Select(item => JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(item)))
                                     .ToList();
            Assert.Single(allEntities);
            Assert.DoesNotContain(allEntities, e => e.Id == "4");
        }

        [Fact]
        public void PostgreDbService_CrudOperations_WorkCorrectly()
        {
            // Ensure PostgreSQL is running and accessible with the provided connection string
            // and the 'openschema' database exists.
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=mysecretpassword;Database=openschema";
            var service = new PostgreDbService(connectionString);
            var collectionName = "TestCollectionPostgre"; // Use a distinct table name for tests

            // Clean up previous test data if any
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand($"DROP TABLE IF EXISTS \"{collectionName}\"", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Test Insert
            var entity1 = new TestEntity { Id = "5", Name = "Eve", Age = 50 };
            Assert.True(service.Insert(collectionName, entity1));

            var entity2 = new TestEntity { Id = "6", Name = "Frank", Age = 45 };
            Assert.True(service.Insert(collectionName, entity2));

            // Test GetAll
            var allEntities = service.GetAll(collectionName)
                                     .Select(item => JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(item)))
                                     .ToList();
            Assert.Equal(2, allEntities.Count);
            Assert.Contains(allEntities, e => e.Id == "5" && e.Name == "Eve");
            Assert.Contains(allEntities, e => e.Id == "6" && e.Name == "Frank");

            // Test GetByField
            var retrievedEntity = JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(service.GetByField(collectionName, "Name", "Eve")));
            Assert.NotNull(retrievedEntity);
            Assert.Equal("5", retrievedEntity.Id);

            // Test Update
            entity1.Age = 51;
            Assert.True(service.Update(collectionName, "Id", "5", entity1));
            retrievedEntity = JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(service.GetByField(collectionName, "Id", "5")));
            Assert.NotNull(retrievedEntity);
            Assert.Equal(51, retrievedEntity.Age);

            // Test Delete
            Assert.True(service.Delete(collectionName, "Id", "6"));
            allEntities = service.GetAll(collectionName)
                                     .Select(item => JsonConvert.DeserializeObject<TestEntity>(JsonConvert.SerializeObject(item)))
                                     .ToList();
            Assert.Single(allEntities);
            Assert.DoesNotContain(allEntities, e => e.Id == "6");
        }

        [Fact]
        public void MongoDbService_CrudOperations_WorkCorrectly()
        {
            // Ensure MongoDB is running locally or adjust connection string
            var connectionString = "mongodb://localhost:27017";
            var dbName = "openschema_test_db";
            var collectionName = "TestCollectionMongo";

            // Register class map for TestEntity
            if (!MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(TestEntity)))
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<TestEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            // Clean up previous test data if any
            database.DropCollection(collectionName);

            var service = new MongoDbService(connectionString, dbName);

            // Test Insert
            var entity1 = new TestEntity { Name = "Grace", Age = 28 }; // Id will be generated by MongoDB
            Assert.True(service.Insert(collectionName, entity1));

            var entity2 = new TestEntity { Name = "Heidi", Age = 33 }; // Id will be generated by MongoDB
            Assert.True(service.Insert(collectionName, entity2));

            // Retrieve entities to get their MongoDB-generated IDs
            var insertedEntity1 = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TestEntity>((MongoDB.Bson.BsonDocument)service.GetByField(collectionName, "Name", "Grace"));
            var insertedEntity2 = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TestEntity>((MongoDB.Bson.BsonDocument)service.GetByField(collectionName, "Name", "Heidi"));

            Assert.NotNull(insertedEntity1);
            Assert.NotNull(insertedEntity2);

            // Test GetAll
            var allEntities = service.GetAll(collectionName)
                                     .Select(item => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TestEntity>((MongoDB.Bson.BsonDocument)item))
                                     .ToList();
            Assert.Equal(2, allEntities.Count);
            Assert.Contains(allEntities, e => e.Id == insertedEntity1.Id && e.Name == "Grace");
            Assert.Contains(allEntities, e => e.Id == insertedEntity2.Id && e.Name == "Heidi");

            // Test GetByField
            var retrievedEntity = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TestEntity>((MongoDB.Bson.BsonDocument)service.GetByField(collectionName, "Name", "Grace"));
            Assert.NotNull(retrievedEntity);
            Assert.Equal(insertedEntity1.Id, retrievedEntity.Id);

            // Test Update
            insertedEntity1.Age = 29;
            Assert.True(service.Update(collectionName, "Id", insertedEntity1.Id, insertedEntity1));
            retrievedEntity = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TestEntity>((MongoDB.Bson.BsonDocument)service.GetByField(collectionName, "Id", insertedEntity1.Id));
            Assert.NotNull(retrievedEntity);
            Assert.Equal(29, retrievedEntity.Age);

            // Test Delete
            Assert.True(service.Delete(collectionName, "Id", insertedEntity2.Id));
            allEntities = service.GetAll(collectionName)
                                     .Select(item => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TestEntity>((MongoDB.Bson.BsonDocument)item))
                                     .ToList();
            Assert.Single(allEntities);
            Assert.DoesNotContain(allEntities, e => e.Id == insertedEntity2.Id);
        }

        [Fact]
        public void LiteDbService_CrudOperations_WorkCorrectly()
        {
            // Use a temporary file for LiteDB testing
            var dbPath = Path.Combine(Path.GetTempPath(), $"openschema_test_{Guid.NewGuid()}.db");
            var collectionName = "TestCollectionLite";

            // Clean up previous test data if any
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }

            using (var service = new LiteDbService(dbPath))
            {
                // Configure LiteDB BsonMapper for TestEntity
                LiteDB.BsonMapper.Global.Entity<TestEntity>().Id(x => x.Id);

            // Test Insert
            var entity1 = new TestEntity { Id = "9", Name = "Ivan", Age = 22 };
            Assert.True(service.Insert(collectionName, entity1));

            var entity2 = new TestEntity { Id = "10", Name = "Julia", Age = 38 };
            Assert.True(service.Insert(collectionName, entity2));

            // Test GetAll
            var allEntities = service.GetAll(collectionName)
                                     .Select(item => LiteDB.BsonMapper.Global.ToObject<TestEntity>((LiteDB.BsonDocument)item))
                                     .ToList();
            Assert.Equal(2, allEntities.Count);
            Assert.Contains(allEntities, e => e.Id == "9" && e.Name == "Ivan");
            Assert.Contains(allEntities, e => e.Id == "10" && e.Name == "Julia");

            // Test GetByField
            var retrievedEntity = LiteDB.BsonMapper.Global.ToObject<TestEntity>((LiteDB.BsonDocument)service.GetByField(collectionName, "Name", "Ivan"));
            Assert.NotNull(retrievedEntity);
            Assert.Equal("9", retrievedEntity.Id);

            // Test Update
            entity1.Age = 23;
            Assert.True(service.Update(collectionName, "Id", "9", entity1));
            retrievedEntity = BsonMapper.Global.ToObject<TestEntity>((BsonDocument)service.GetByField(collectionName, "Id", "9"));
            Assert.NotNull(retrievedEntity);
            Assert.Equal(23, retrievedEntity.Age);

            // Test Delete
            Assert.True(service.Delete(collectionName, "Id", "10"));
            allEntities = service.GetAll(collectionName)
                                     .Select(item => LiteDB.BsonMapper.Global.ToObject<TestEntity>((LiteDB.BsonDocument)item))
                                     .ToList();
            Assert.Single(allEntities);
            Assert.DoesNotContain(allEntities, e => e.Id == "10");

            }
            // Clean up the database file
            File.Delete(dbPath);
        }
    }
}
