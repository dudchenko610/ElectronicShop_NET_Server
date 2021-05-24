using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Contexts
{
    public class DbContextMongo
    {
        public IMongoDatabase Database { get; set; }



        public DbContextMongo(IOptions<MongoConnectionOptions> mongoOptions)
        {
            string connectionString = mongoOptions.Value.ConnectionString;
            MongoUrlBuilder connection = new MongoUrlBuilder(connectionString);

            MongoClient client = new MongoClient(connectionString);
            Database = client.GetDatabase(connection.DatabaseName);
        }

        /*
        public async Task<bool> UpdateOneAsyncConcurrently<T>
            (
                IMongoCollection<T> mongoCollection,
                MongoDbConcurrentModel entity,
                UpdateDefinition<T> updateDefinition
            ) where T : MongoDbConcurrentModel
        {

            string oldAccessId = entity.AccessId + "";
            entity.AccessId = BuildAccessId();

            UpdateResult updateResult
                = await mongoCollection.UpdateOneAsync<T>(x => x.Id == entity.Id && oldAccessId == x.AccessId, updateDefinition);

            return updateResult.ModifiedCount > 0;
        }
        */

        private static string BuildAccessId()
        {
            return Guid.NewGuid().ToString()
                .Replace("-", "")
                .Replace("{", "")
                .Replace("}", "")
                .Substring(0, 15);
        }
    }
}
