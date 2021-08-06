using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace Matr.Utilities.Extensions.Mongo.UnitTests
{
    [TestClass]
    public class CollectionNameTests : MongoDatabaseExtensionsTestsBase
    {
        [TestMethod]
        public void CollectionNamesAsList_CollectionInDatabase_ListContainsCollectionName()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var result = database.CollectionNamesAsList();

            result.Should().Contain(collectionName);
        }

        [TestMethod]
        public void CollectionNamesAsList_CollectionNotInDatabase_ListDoesNotContainsCollectionName()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();

            var result = database.CollectionNamesAsList();

            result.Should().NotContain(collectionName);
        }

        [TestMethod]
        public async Task CollectionNamesAsListAsync_CollectionInDatabase_ListShouldContainsCollectionName()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var result = await database.CollectionNamesAsListAsync();

            result.Should().Contain(collectionName);
        }

        [TestMethod]
        public async Task CollectionNamesAsListAsync_CollectionNotInDatabase_ListShouldNotContainsCollectionName()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();

            var result = await database.CollectionNamesAsListAsync();

            result.Should().NotContain(collectionName);
        }

        [TestMethod]
        public void CollectionExists_DoesNotExist_ShouldBeFalse()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();

            database.CollectionExists(collectionName).Should().BeFalse();
        }

        [TestMethod]
        public void CollectionExists_DoesExist_ShouldBeTrue()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            database.GetCollection<BsonDocument>(collectionName).InsertOne(new BsonDocument { });

            database.CollectionExists(collectionName).Should().BeTrue();
        }

        [TestMethod]
        public async Task CollectionExistsAsync_DoesNotExist_ShouldBeFalse()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();

            (await database.CollectionExistsAsync(collectionName)).Should().BeFalse();
        }

        [TestMethod]
        public async Task CollectionExistsAsync_DoesExist_ShouldBeTrue()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            database.GetCollection<BsonDocument>(collectionName).InsertOne(new BsonDocument { });

            (await database.CollectionExistsAsync(collectionName)).Should().BeTrue();
        }
    }
}