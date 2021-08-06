using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matr.Utilities.Extensions.Mongo
{
    public static partial class MongoDatabaseExtensions
    {
        /// <summary>
        /// Creates a <see cref="List{string}"/> of all collections in the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database">The database to get the collection names from.</param>
        /// <returns>A <see cref="List{string}"/> that contains all collection names in the given <paramref name="database"/>.</returns>
        public static List<string> CollectionNamesAsList(this IMongoDatabase database)
            => database.ListCollectionNames().ToList();

        /// <summary>
        /// Creates a <see cref="Task{List{string}}"/> of all collections in the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database">The database to get the collection names from.</param>
        /// <returns>A <see cref="Task{List{string}}"/> that contains all collection names in the given <paramref name="database"/>.</returns>
        public static async Task<List<string>> CollectionNamesAsListAsync(this IMongoDatabase database)
            => (await database.ListCollectionNamesAsync()).ToList();

        /// <summary>
        /// Creates a <see cref="List{string}"/> of all databases in the given <paramref name="client"/>
        /// </summary>
        /// <param name="client">The client to get the database names from.</param>
        /// <returns>A <see cref="List{string}"/> that contains all database names in the given <paramref name="client"/>.</returns>
        public static List<string> DatabaseNamesAsList(this IMongoClient client)
            => client.ListDatabaseNames().ToList();

        /// <summary>
        /// Creates a <see cref="List{string}"/> of all databases in the given <paramref name="client"/>
        /// </summary>
        /// <param name="client">The client to get the database names from.</param>
        /// <returns>A <see cref="List{string}"/> that contains all database names in the given <paramref name="client"/>.</returns>
        public static async Task<List<string>> DatabaseNamesAsListAsync(this IMongoClient client)
            => (await client.ListDatabaseNamesAsync()).ToList();

        /// <summary>
        /// Droppes all collections in the given <paramref name="collectionNames"/> if left empty, it will remove all collections in the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database">The database to drop the <paramref name="collectionNames"/> from.</param>
        /// <param name="collectionNames">The collection names to drop. If left empty, it will remove all collections in the given <paramref name="database"/></param>
        public static void DropAllCollections(this IMongoDatabase database, params string[] collectionNames)
        {
            if (!collectionNames.Any())
                collectionNames = database.CollectionNamesAsList().ToArray();

            collectionNames.ToList()
                .ForEach(collectionName => database.DropCollection(collectionName));
        }

        /// <summary>
        /// Droppes all collections in the given <paramref name="collectionNames"/> if left empty, it will remove all collections in the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database">The database to drop the <paramref name="collectionNames"/> from.</param>
        /// <param name="collectionNames">The collection names to drop. If left empty, it will remove all collections in the given <paramref name="database"/></param>
        public static async Task DropAllCollectionsAsync(this IMongoDatabase database, params string[] collectionNames)
        {
            if (!collectionNames.Any())
                collectionNames = (await database.CollectionNamesAsListAsync()).ToArray();

#if NETCOREAPP3_1
            var waitList = new List<Task>();
#else
            List<Task> waitList = new();
#endif

            collectionNames.ToList()
                .ForEach(collectionName => waitList.Add(database.DropCollectionAsync(collectionName)));

            Task.WaitAll(waitList.ToArray());
        }

        /// <summary>
        /// Droppes all collections in the given <paramref name="databaseNames"/> if left empty, it will remove all collections in the given <paramref name="client"/>.
        /// </summary>
        /// <param name="client">The client to drop the <paramref name="databaseNames"/> from.</param>
        /// <param name="databaseNames">The database names to drop. If left empty, it will remove all databases in the given <paramref name="client"/>.</param>
        public static void DropAllDatabases(this IMongoClient client, params string[] databaseNames)
        {
            if (!databaseNames.Any())
                databaseNames = client.DatabaseNamesAsList()
                    .Where(x => x != "admin")
                    .Where(x => x != "local")
                    .ToArray();

            databaseNames.ToList()
                .ForEach(databaseName => client.DropDatabase(databaseName));
        }

        /// <summary>
        /// Droppes all collections in the given <paramref name="databaseNames"/> if left empty, it will remove all collections in the given <paramref name="client"/>.
        /// </summary>
        /// <param name="client">The client to drop the <paramref name="databaseNames"/> from.</param>
        /// <param name="databaseNames">The database names to drop. If left empty, it will remove all databases in the given <paramref name="client"/>.</param>
        /// <returns></returns>
        public static async Task DropAllDatabasesAsync(this IMongoClient client, params string[] databaseNames)
        {
            if (!databaseNames.Any())
                databaseNames = (await client.DatabaseNamesAsListAsync())
                    .Where(x => x != "admin")
                    .Where(x => x != "local")
                    .ToArray();

#if NETCOREAPP3_1
            var waitList = new List<Task>();
#else
            List<Task> waitList = new();
#endif
            databaseNames.ToList()
                .ForEach(databaseName => waitList.Add(client.DropDatabaseAsync(databaseName)));

            Task.WaitAll(waitList.ToArray());
        }

        /// <summary>
        /// Checks to see if <paramref name="source"/> contains a collection with the given <paramref name="collectionName"/>.
        /// </summary>
        /// <param name="source">The mongo database to check.</param>
        /// <param name="collectionName">Name of the collection to check.</param>
        /// <returns>True if contains the collection with the <paramref name="collectionName"/>, otherwise false</returns>
        public static bool CollectionExists(this IMongoDatabase source, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            return source.ListCollectionNames(options).Any();
        }

        /// <summary>
        /// Checks to see if <paramref name="source"/> contains a collection with the given <paramref name="collectionName"/>.
        /// </summary>
        /// <param name="source">The mongo database to check.</param>
        /// <param name="collectionName">Name of the collection to check.</param>
        /// <returns>True if contains the collection with the <paramref name="collectionName"/>, otherwise false</returns>
        public static async Task<bool> CollectionExistsAsync(this IMongoDatabase source, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            var collections = await source.ListCollectionNamesAsync(options);
            return collections.Any();
        }
    }
}