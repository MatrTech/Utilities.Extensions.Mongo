using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace Matr.Utilities.Extensions.Mongo.UnitTests
{
    [TestClass]
    public class DatabaseNameTests : MongoDatabaseExtensionsTestsBase
    {
        [TestMethod]
        public void DatabaseNamesAsList_DatabaseDoesExist_ShouldContain()
        {
            var databaseName = Guid.NewGuid().ToString();
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var result = client.DatabaseNamesAsList();

            result.Should().Contain(databaseName);
        }

        [TestMethod]
        public void DatabaseNamesAsList_DatabaseDoesNotExist_ShouldNotContain()
        {
            var databaseName = Guid.NewGuid().ToString();

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public async Task DatabaseNamesAsListAsync_DatabaseDoesNotExist_ShouldNotContain()
        {
            var databaseName = Guid.NewGuid().ToString();

            var result = await client.DatabaseNamesAsListAsync();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public async Task DatabaseNamesAsListAsync_DatabaseDoesExist_ShouldContain()
        {
            var databaseName = Guid.NewGuid().ToString();
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var result = await client.DatabaseNamesAsListAsync();

            result.Should().Contain(databaseName);
        }
    }
}