using MongoDB.Driver;
using pod_app.DataLayer;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class MongoDbServiceTest : IDisposable
    {
        private readonly IPodcastRepository service;

        public MongoDbServiceTest()
        {
            service = new PodcastServiceInMemory();
        }

        [Fact]
        public void TestGetAllFeeds()
        {
            // Base count
            int base_count = service.GetAllFeeds().Count;

            // Insert test data
            var feed1 = new Podcast
            {
                Category = "Tech",
                Episodes = new List<Episode>()
            };

            var feed2 = new Podcast
            {
                Category = "News",
                Episodes = new List<Episode>()
            };

            service.PushFeed(feed1);
            service.PushFeed(feed2);

            // Act
            var data = service.GetAllFeeds();

            // Validate data
            Assert.NotNull(data);
            Assert.Equal(base_count + 2, data.Count);
            Assert.Contains(data, f => f.Category == "Tech");
            Assert.Contains(data, f => f.Category == "News");

            // Remove data
            service.DeleteFeed(feed1);
            service.DeleteFeed(feed2);
        }

        [Fact]
        public void TestGetFeed()
        {
            // Insert single feed
            var feed = new Podcast
            {
                Category = "Music",
                Episodes = new List<Episode>()
            };

            service.PushFeed(feed);
            Assert.False(string.IsNullOrWhiteSpace(feed.Id)); // Mongo sets Id

            // Act
            var loaded = service.GetFeed(feed.Id);

            // Validate data
            Assert.NotNull(loaded);
            Assert.Equal(feed.Id, loaded.Id);
            Assert.Equal("Music", loaded.Category);

            // Remove data
            service.DeleteFeed(feed);
            Assert.Null(service.GetFeed(feed.Id));
        }

        [Fact]
        public void TestPushFeed()
        {
            // Insert single feed
            var feed = new Podcast
            {
                Category = "Comedy",
                Episodes = new List<Episode>()
            };

            service.PushFeed(feed);

            // Validate data
            var fromDb = service.GetFeed(feed.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Comedy", fromDb.Category);

            // remove data
            service.DeleteFeed(feed);
        }

        [Fact]
        public void TestDeleteFeed()
        {
            // Insert single feed
            var feed = new Podcast
            {
                Category = "DeleteMe",
                Episodes = new List<Episode>()
            };

            service.PushFeed(feed);
            Assert.NotNull(service.GetFeed(feed.Id));

            // remove data
            service.DeleteFeed(feed);

            // Ensure data is deleted
            var deleted = service.GetFeed(feed.Id);
            Assert.Null(deleted);
        }

        public void Dispose()
        {
            // CLear database 
            service.GetAllFeeds().ForEach(f =>  service.DeleteFeed(f));
        }
    }
}
