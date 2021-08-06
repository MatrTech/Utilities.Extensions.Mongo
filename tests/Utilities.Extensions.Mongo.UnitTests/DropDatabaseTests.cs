using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace Matr.Utilities.Extensions.Mongo.UnitTests
{
    [TestClass]
    public class DropDatabaseTests : MongoDatabaseExtensionsTestsBase
    {
        [TestMethod]
        public void DropAllCollections_DatabaseWithCollections_ShouldBeEmpty()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = database.CollectionNamesAsList();
            resultBeforeDrop.Should().Contain(collectionName);

            database.DropAllCollections();

            var result = database.CollectionNamesAsList();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void DropAllCollections_RemoveOneCollection_ShouldNotContainRemovedButNotEmpty()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var otherCollectionName = Guid.NewGuid().ToString();
            var otherCollection = database.GetCollection<BsonDocument>(otherCollectionName);
            otherCollection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = database.CollectionNamesAsList();
            resultBeforeDrop.Should().Contain(collectionName);
            resultBeforeDrop.Should().Contain(otherCollectionName);

            database.DropAllCollections(collectionName);

            var result = database.CollectionNamesAsList();
            result.Should().NotBeEmpty();
            result.Should().Contain(otherCollectionName);
        }

        [TestMethod]
        public async Task DropAllCollectionsAsync_DatabaseWithCollections_ShouldBeEmpty()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = database.CollectionNamesAsList();
            resultBeforeDrop.Should().Contain(collectionName);

            await database.DropAllCollectionsAsync();

            var result = database.CollectionNamesAsList();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task DropAllCollectionsAsync_RemoveOneCollection_ShouldNotContainRemovedButNotEmpty()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var otherCollectionName = Guid.NewGuid().ToString();
            var otherCollection = database.GetCollection<BsonDocument>(otherCollectionName);
            otherCollection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = database.CollectionNamesAsList();
            resultBeforeDrop.Should().Contain(collectionName);
            resultBeforeDrop.Should().Contain(otherCollectionName);

            await database.DropAllCollectionsAsync(collectionName);

            var result = database.CollectionNamesAsList();
            result.Should().NotBeEmpty();
            result.Should().Contain(otherCollectionName);
        }

        [TestMethod]
        public void DropAllDatabases_ExistingDatabase_ShouldOnlyContainAdminAndLocal()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().Contain(databaseName);

            client.DropAllDatabases();

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public void DropAllDatabases_DatabaseDoesNotExist_ShouldOnlyContainAdminAndLocal()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";

            var resultBeforeDrop = client.DatabaseNamesAsList();

            resultBeforeDrop.Contains(databaseName);

            client.DropAllDatabases(databaseName);

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public void DropAllDatabases_DropSpecificDatabase_ShouldContainAllExceptDropped()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var otherDatabaseName = $"test-database-{Guid.NewGuid()}";
            otherDatabase = client.GetDatabase(otherDatabaseName);
            var otherCollectionName = Guid.NewGuid().ToString();
            var otherCollection = otherDatabase.GetCollection<BsonDocument>(otherCollectionName);
            otherCollection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().Contain(databaseName);
            resultBeforeDrop.Should().Contain(otherDatabaseName);

            client.DropAllDatabases(databaseName);

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
            result.Should().Contain(otherDatabaseName);
        }

        [TestMethod]
        public async Task DropAllDatabasesAsync_ExistingDatabase_ShouldNotContainDatabase()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().Contain(databaseName);

            await client.DropAllDatabasesAsync();

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public async Task DropAllDatabasesAsync_DatabaseDoesNotExist_ShouldOnlyContainAdminAndLocal()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().NotContain(databaseName);

            await client.DropAllDatabasesAsync(databaseName);

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public async Task DropAllDatabasesAsync_DropSpecificDatabase_ShouldContainAllExceptDropped()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var otherDatabaseName = $"test-database-{Guid.NewGuid()}";
            otherDatabase = client.GetDatabase(otherDatabaseName);
            var otherCollectionName = Guid.NewGuid().ToString();
            var otherCollection = otherDatabase.GetCollection<BsonDocument>(otherCollectionName);
            otherCollection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().Contain(databaseName);
            resultBeforeDrop.Should().Contain(otherDatabaseName);

            await client.DropAllDatabasesAsync(databaseName);

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
            result.Should().Contain(otherDatabaseName);
        }
    }
}