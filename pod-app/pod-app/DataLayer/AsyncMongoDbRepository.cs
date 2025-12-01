using MongoDB.Bson;
using MongoDB.Driver;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{
    // Async methods for the repository using Builders from MongoDB-Driver and transactions

    public class MongoDbRepositoryAsync : IPodcastRepositoryAsync  // Implementing the interface
    {
        private readonly MongoClient client;
        private readonly IMongoCollection<Podcast> feedCollection;
        private readonly IMongoCollection<Episode> podcastCollection;

        public MongoDbRepositoryAsync(string connection_str)
        {
            var settings = MongoClientSettings.FromConnectionString(connection_str);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            client = new MongoClient(settings);

            var database = client.GetDatabase("Smultron-storage");  
            feedCollection = database.GetCollection<Podcast>("Podcasts");
            podcastCollection = database.GetCollection<Episode>("Episodes");
        }

        // Adds the podcast to the collection
        public async Task PushFeedAsync(Podcast feed)
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

        // Gets a podcast based on the id
        public async Task<Podcast?> GetFeedAsync(string id)
        {
            var filter = Builders<Podcast>.Filter.Eq(f => f.Id, id);
            return await feedCollection.Find(filter).FirstOrDefaultAsync();
        }

        // Updates the podcast
        public async Task UpdateFeedAsync(Podcast feed)
        {
            using var session = await client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var filter = Builders<Podcast>.Filter.Eq(f => f.Id, feed.Id);
                await feedCollection.ReplaceOneAsync(session, filter, feed);

                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        // Deleting a podcast
        public async Task DeleteFeedAsync(Podcast feed)
        {
            using var session = await client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var filter = Builders<Podcast>.Filter.Eq(f => f.Id, feed.Id);
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

        // Gets all podcasts from the collection
        public async Task<List<Podcast>> GetAllFeedsAsync()
        {
            return await feedCollection
                .Find(Builders<Podcast>.Filter.Empty)
                .ToListAsync();
        }



        public Task<List<Episode>> GetPodcastsAsync(Podcast feed)
        {
            throw new NotImplementedException();
        }

        
    }
}
