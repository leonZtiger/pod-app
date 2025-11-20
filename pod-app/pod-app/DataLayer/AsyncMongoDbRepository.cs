using MongoDB.Bson;
using MongoDB.Driver;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{
    public class MongoDbRepositoryAsync
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

        public async Task AddFeedAsync(PodFlow feed)
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


            }
        }

            
        public async Task AddPodAsync(PodModel pod)
        {
            using var session = await client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                await podcastCollection.InsertOneAsync(session, pod);
                await session.CommitTransactionAsync();
            }

            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task DeletePodAsync(PodModel pod)
        {
            using var session = await client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var filter = Builders<PodModel>.Filter.Eq(p => p.Id, pod.Id);
                var result = await podcastCollection.DeleteOneAsync(session, filter);

                if (result.DeletedCount == 0)
                {
                    await session.AbortTransactionAsync();
                    throw new KeyNotFoundException("No pod found with specified Id.");
                }

                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }


        public async Task UpdateModelAsync(PodModel model)
        {
            using var session = await client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var filter = Builders<PodModel>.Filter.Eq("_id", ObjectId.Parse(model.Id));

                var res = await podcastCollection.ReplaceOneAsync(session, filter, model);

                if (res.MatchedCount == 0)
                {
                    await session.AbortTransactionAsync();
                    throw new KeyNotFoundException("Could not find MongoDB document with specified key.");
                }

                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }


        

       
    }
}
