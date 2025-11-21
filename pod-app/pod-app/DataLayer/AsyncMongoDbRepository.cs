using MongoDB.Bson;
using MongoDB.Driver;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{
    public class MongoDbRepositoryAsync : IPodcastRepositoryAsync
    {
        private readonly MongoClient client;
        private readonly IMongoCollection<PodFlow> feedCollection;
        private readonly IMongoCollection<PodModel> podcastCollection;

        public MongoDbRepositoryAsync(string connection_str)
        {
            var settings = MongoClientSettings.FromConnectionString(connection_str);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            client = new MongoClient(settings);

            var database = client.GetDatabase("Smultron-storage");
            feedCollection = database.GetCollection<PodFlow>("Podcasts");
            podcastCollection = database.GetCollection<PodModel>("Episodes");
        }

        public async Task PushFeedAsync(PodFlow feed)
        {
            using var session = await client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                await feedCollection.InsertOneAsync(session, feed);
                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task<PodFlow?> GetFeedAsync(string id)
        {
            var filter = Builders<PodFlow>.Filter.Eq(f => f.Id, id);
            return await feedCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateFeedAsync(PodFlow feed)
        {
            using var session = await client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var filter = Builders<PodFlow>.Filter.Eq(f => f.Id, feed.Id);
                await feedCollection.ReplaceOneAsync(session, filter, feed);

                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }



        public async Task DeleteFeedAsync(PodFlow feed)
        {
            using var session = await client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var filter = Builders<PodFlow>.Filter.Eq(f => f.Id, feed.Id);
                var result = await feedCollection.DeleteOneAsync(session, filter);


                if (result.DeletedCount == 0)
                {
                    await session.AbortTransactionAsync();
                    throw new KeyNotFoundException("No feed found with specified Id.");
                }

                await session.CommitTransactionAsync();
            }

            catch
            {
                await session.AbortTransactionAsync();
                throw;


            }
        }

        public async Task<List<PodFlow>> GetAllFeedsAsync()
        {
            return await feedCollection
                .Find(Builders<PodFlow>.Filter.Empty)
                .ToListAsync();
        }



        public Task<List<PodModel>> GetPodcastsAsync(PodFlow feed)
        {
            throw new NotImplementedException();
        }
    }
}
