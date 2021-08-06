using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Matr.Utilities.Extensions.Mongo.UnitTests
{
    [TestClass]
    public class MongoDatabaseExtensionsTestsBase
    {
        protected static MongoClient client = null!;
        protected IMongoDatabase? database;
        protected IMongoDatabase? otherDatabase;

        public MongoDatabaseExtensionsTestsBase()
        {
            client = new MongoClient("mongodb://localhost:27017");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            client.DropAllDatabases();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            database?.DropAllCollections();
            otherDatabase?.DropAllCollections();
        }
    }
}