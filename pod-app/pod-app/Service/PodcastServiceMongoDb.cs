using MongoDB.Bson;
using MongoDB.Driver;
using pod_app.DataLayer;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.Service
{
    public class PodcastServiceMongoDb : IPodcastDataService
    {
        private readonly IMongoCollection<Podcast> feedCollection;
        private readonly IMongoCollection<Episode> podcastCollection;

        public PodcastServiceMongoDb(string connection_str)
        {

            var settings = MongoClientSettings.FromConnectionString(connection_str);

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);
     
            this.feedCollection = client.GetDatabase("Smultron-storage").GetCollection<Podcast>("Podcasts");
            this.podcastCollection = client.GetDatabase("Smultron-storage").GetCollection<Episode>("Episodes");

        }

        public void PushFeed(Podcast feed)
        {
            feedCollection.InsertOne(feed);
        }

        public void DeleteFeed(Podcast feed)
        {
            FilterDefinition<Podcast> filter = new ObjectFilterDefinition<Podcast>(feed);

            feedCollection.DeleteOne(filter);
        }

        public void DeletePod(Episode pod)
        {
            FilterDefinition<Episode> filter = new ObjectFilterDefinition<Episode>(pod);

            podcastCollection.DeleteOne(filter);
        }

        public List<Podcast> GetAllFeeds()
        {
            return feedCollection.Find(Builders<Podcast>.Filter.Empty).ToList();
        }

        public void PushPod(Episode pod)
        {
            podcastCollection.InsertOne(pod);
        }

        public Podcast? GetFeed(string id)
        {
            return feedCollection.Find(f => f.Id == id).FirstOrDefault();
        }

      

        public List<Episode> GetPodcasts(Podcast feed)
        {
            throw new NotImplementedException();
        }
    }
}
