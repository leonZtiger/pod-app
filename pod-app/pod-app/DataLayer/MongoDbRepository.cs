using MongoDB.Bson;
using MongoDB.Driver;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace pod_app.DataLayer
{
    public class MongoDbRepository : IPodcastRepository
    {
        
        private readonly IMongoCollection<PodFlow> feedCollection;
        private readonly IMongoCollection<PodModel> podcastCollection;

        public MongoDbRepository(string connection_str)
        {

            var settings = MongoClientSettings.FromConnectionString(connection_str);

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);
     
            feedCollection = client.GetDatabase("Smultron-storage").GetCollection<PodFlow>("Podcasts");
            podcastCollection = client.GetDatabase("Smultron-storage").GetCollection<PodModel>("Episodes");

        }

        public void PushFeed(PodFlow feed)
        {
            feedCollection.InsertOne(feed);
        }


        public void DeleteFeed(PodFlow feed)
        {
            FilterDefinition<PodFlow> filter = new ObjectFilterDefinition<PodFlow>(feed);

            feedCollection.DeleteOne(filter);
        }

        public void DeletePod(PodModel pod)
        {
            FilterDefinition<PodModel> filter = new ObjectFilterDefinition<PodModel>(pod);

            podcastCollection.DeleteOne(filter);
        }

        public List<PodFlow> GetAllFeeds()
        {
            return feedCollection.Find(Builders<PodFlow>.Filter.Empty).ToList();
        }

        public void PushPod(PodModel pod)
        {
            podcastCollection.InsertOne(pod);
        }

        public PodFlow? GetFeed(string id)
        {
            return feedCollection.Find(f => f.Id == id).FirstOrDefault();
        }

        public PodModel? GetPodcast(string id)
        {
            return podcastCollection.Find(f => f.Id == id).FirstOrDefault();

        }

        public void UpdateModel(PodModel model)
        {
            FilterDefinition<PodModel> filter = Builders<PodModel>.Filter.Eq("_id", ObjectId.Parse(model.Id));

            ReplaceOneResult res =  podcastCollection.ReplaceOne(filter, model);

            // No docs found throw key not found
            if(res.MatchedCount == 0)
            {
                throw new KeyNotFoundException("Could not find MongoDb document with Specified key.");
            }
        }

        public List<PodModel> GetPodcasts(PodFlow feed)
        {
            throw new NotImplementedException();
        }
    }
}
